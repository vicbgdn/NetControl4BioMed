﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetControl4BioMed.Data;
using NetControl4BioMed.Data.Enumerations;
using NetControl4BioMed.Data.Models;
using NetControl4BioMed.Helpers.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NetControl4BioMed.Helpers.Extensions;
using System.Threading.Tasks;
using NetControl4BioMed.Helpers.Exceptions;

namespace NetControl4BioMed.Helpers.Tasks
{
    /// <summary>
    /// Implements a task to update edges in the database.
    /// </summary>
    public class EdgesTask
    {
        /// <summary>
        /// Gets or sets the items to be updated.
        /// </summary>
        public IEnumerable<EdgeInputModel> Items { get; set; }

        /// <summary>
        /// Creates the items in the database.
        /// </summary>
        /// <param name="serviceProvider">The application service provider.</param>
        /// <param name="token">The cancellation token for the task.</param>
        public async Task CreateAsync(IServiceProvider serviceProvider, CancellationToken token)
        {
            // Check if there weren't any valid items found.
            if (Items == null)
            {
                // Throw an exception.
                throw new TaskException("No valid items could be found with the provided data.");
            }
            // Check if the exception item should be shown.
            var showExceptionItem = Items.Count() > 1;
            // Get the total number of batches.
            var count = Math.Ceiling((double)Items.Count() / ApplicationDbContext.BatchSize);
            // Go over each batch.
            for (var index = 0; index < count; index++)
            {
                // Check if the cancellation was requested.
                if (token.IsCancellationRequested)
                {
                    // Break.
                    break;
                }
                // Get the items in the current batch.
                var batchItems = Items
                    .Skip(index * ApplicationDbContext.BatchSize)
                    .Take(ApplicationDbContext.BatchSize);
                // Get the IDs of the items in the current batch.
                var batchIds = batchItems
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id);
                // Check if any of the IDs are repeating in the list.
                if (batchIds.Distinct().Count() != batchIds.Count())
                {
                    // Throw an exception.
                    throw new TaskException("Two or more of the manually provided IDs are duplicated.");
                }
                // Get the IDs of the related entities that appear in the current batch.
                var batchDatabaseIds = batchItems
                    .Where(item => item.DatabaseEdges != null)
                    .Select(item => item.DatabaseEdges)
                    .SelectMany(item => item)
                    .Where(item => item.Database != null)
                    .Select(item => item.Database)
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id)
                    .Distinct();
                var batchDatabaseEdgeFieldIds = batchItems
                    .Where(item => item.DatabaseEdgeFieldEdges != null)
                    .Select(item => item.DatabaseEdgeFieldEdges)
                    .SelectMany(item => item)
                    .Where(item => item.DatabaseEdgeField != null)
                    .Select(item => item.DatabaseEdgeField)
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id)
                    .Distinct();
                var batchNodeIds = batchItems
                    .Where(item => item.EdgeNodes != null)
                    .Select(item => item.EdgeNodes)
                    .SelectMany(item => item)
                    .Where(item => item.Node != null)
                    .Select(item => item.Node)
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id)
                    .Distinct();
                // Define the list of items to get.
                var validBatchIds = new List<string>();
                var databaseEdgeFields = new List<DatabaseEdgeField>();
                var databases = new List<Database>();
                var nodes = new List<Node>();
                // Use a new scope.
                using (var scope = serviceProvider.CreateScope())
                {
                    // Use a new context instance.
                    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    // Get the related entities that appear in the current batch.
                    databaseEdgeFields = context.DatabaseEdgeFields
                        .Where(item => item.Database.DatabaseType.Name != "Generic")
                        .Where(item => batchDatabaseEdgeFieldIds.Contains(item.Id))
                        .ToList();
                    databases = context.Databases
                        .Where(item => item.DatabaseType.Name != "Generic")
                        .Where(item => batchDatabaseIds.Contains(item.Id))
                        .Concat(context.DatabaseEdgeFields
                            .Where(item => item.Database.DatabaseType.Name != "Generic")
                            .Where(item => batchDatabaseEdgeFieldIds.Contains(item.Id))
                            .Select(item => item.Database))
                        .Distinct()
                        .ToList();
                    nodes = context.Nodes
                        .Where(item => !item.DatabaseNodes.Any(item1 => item1.Database.DatabaseType.Name == "Generic"))
                        .Where(item => batchNodeIds.Contains(item.Id))
                        .ToList();
                    // Get the valid IDs, that do not appear in the database.
                    validBatchIds = batchIds
                        .Except(context.Edges
                            .Where(item => batchIds.Contains(item.Id))
                            .Select(item => item.Id))
                        .ToList();
                }
                // Save the items to add.
                var edgesToAdd = new List<Edge>();
                // Go over each of the items.
                foreach (var batchItem in batchItems)
                {
                    // Check if the ID of the item is not valid.
                    if (!string.IsNullOrEmpty(batchItem.Id) && !validBatchIds.Contains(batchItem.Id))
                    {
                        // Continue.
                        continue;
                    }
                    // Check if there were no edge nodes provided.
                    if (batchItem.EdgeNodes == null || !batchItem.EdgeNodes.Any())
                    {
                        // Throw an exception.
                        throw new TaskException("There were no edge nodes provided.", showExceptionItem, batchItem);
                    }
                    // Get the edge nodes.
                    var edgeNodes = batchItem.EdgeNodes
                        .Where(item => item.Node != null)
                        .Where(item => !string.IsNullOrEmpty(item.Node.Id))
                        .Where(item => item.Type == "Source" || item.Type == "Target")
                        .Select(item => (item.Node.Id, item.Type))
                        .Distinct()
                        .Where(item => nodes.Any(item1 => item1.Id == item.Item1))
                        .Select(item => new EdgeNode
                        {
                            NodeId = item.Item1,
                            Type = EnumerationExtensions.GetEnumerationValue<EdgeNodeType>(item.Item2)
                        });
                    // Check if there were no edge nodes found.
                    if (edgeNodes == null || !edgeNodes.Any(item => item.Type == EdgeNodeType.Source) || !edgeNodes.Any(item => item.Type == EdgeNodeType.Target))
                    {
                        // Throw an exception.
                        throw new TaskException("There were no edge nodes found.", showExceptionItem, batchItem);
                    }
                    // Check if there were no database edges or database edge field edges provided.
                    if ((batchItem.DatabaseEdges == null || !batchItem.DatabaseEdges.Any()) && (batchItem.DatabaseEdgeFieldEdges == null || !batchItem.DatabaseEdgeFieldEdges.Any()))
                    {
                        // Throw an exception.
                        throw new TaskException("There were no database edges or database edge field edges provided.", showExceptionItem, batchItem);
                    }
                    // Get the database edge field edges.
                    var databaseEdgeFieldEdges = batchItem.DatabaseEdgeFieldEdges != null ?
                        batchItem.DatabaseEdgeFieldEdges
                            .Where(item => item.DatabaseEdgeField != null)
                            .Where(item => !string.IsNullOrEmpty(item.DatabaseEdgeField.Id))
                            .Where(item => !string.IsNullOrEmpty(item.Value))
                            .Select(item => (item.DatabaseEdgeField.Id, item.Value))
                            .Distinct()
                            .Where(item => databaseEdgeFields.Any(item1 => item1.Id == item.Item1))
                            .Select(item => new DatabaseEdgeFieldEdge
                            {
                                DatabaseEdgeFieldId = item.Item1,
                                Value = item.Item2
                            }) :
                        Enumerable.Empty<DatabaseEdgeFieldEdge>();
                    // Get the database edges.
                    var databaseEdgeFieldIds = databaseEdgeFieldEdges
                        .Select(item => item.DatabaseEdgeFieldId)
                        .Distinct();
                    var currentDatabaseEdgeFields = databaseEdgeFields
                        .Where(item => databaseEdgeFieldIds.Contains(item.Id));
                    var databaseEdges = batchItem.DatabaseEdges != null ?
                        batchItem.DatabaseEdges
                            .Where(item => item.Database != null)
                            .Where(item => !string.IsNullOrEmpty(item.Database.Id))
                            .Select(item => item.Database.Id)
                            .Concat(currentDatabaseEdgeFields
                                .Select(item => item.Database.Id))
                            .Distinct()
                            .Where(item => databases.Any(item1 => item1.Id == item))
                            .Select(item => new DatabaseEdge
                            {
                                DatabaseId = item,
                            }) :
                        Enumerable.Empty<DatabaseEdge>();
                    // Check if there were no database edges found.
                    if (databaseEdges == null || !databaseEdges.Any())
                    {
                        // Throw an exception.
                        throw new TaskException("There were no database edges found.", showExceptionItem, batchItem);
                    }
                    // Define the new edge.
                    var edge = new Edge
                    {
                        DateTimeCreated = DateTime.UtcNow,
                        Name = string.Concat(nodes.First(item => item.Id == edgeNodes.First(item => item.Type == EdgeNodeType.Source).NodeId).Name, " - ", nodes.First(item => item.Id == edgeNodes.First(item => item.Type == EdgeNodeType.Target).NodeId).Name),
                        Description = batchItem.Description,
                        EdgeNodes = new List<EdgeNode>
                        {
                            edgeNodes.First(item => item.Type == EdgeNodeType.Source),
                            edgeNodes.First(item => item.Type == EdgeNodeType.Target)
                        },
                        DatabaseEdgeFieldEdges = databaseEdgeFieldEdges.ToList(),
                        DatabaseEdges = databaseEdges.ToList()
                    };
                    // Check if there is any ID provided.
                    if (!string.IsNullOrEmpty(batchItem.Id))
                    {
                        // Assign it to the node.
                        edge.Id = batchItem.Id;
                    }
                    // Add the new node to the list.
                    edgesToAdd.Add(edge);
                }
                // Create the items.
                await IEnumerableExtensions.CreateAsync(edgesToAdd, serviceProvider, token);
            }
        }

        /// <summary>
        /// Edits the items in the database.
        /// </summary>
        /// <param name="serviceProvider">The application service provider.</param>
        /// <param name="token">The cancellation token for the task.</param>
        public async Task EditAsync(IServiceProvider serviceProvider, CancellationToken token)
        {
            // Check if there weren't any valid items found.
            if (Items == null)
            {
                // Throw an exception.
                throw new TaskException("No valid items could be found with the provided data.");
            }
            // Check if the exception item should be shown.
            var showExceptionItem = Items.Count() > 1;
            // Get the total number of batches.
            var count = Math.Ceiling((double)Items.Count() / ApplicationDbContext.BatchSize);
            // Go over each batch.
            for (var index = 0; index < count; index++)
            {
                // Check if the cancellation was requested.
                if (token.IsCancellationRequested)
                {
                    // Break.
                    break;
                }
                // Get the items in the current batch.
                var batchItems = Items
                    .Skip(index * ApplicationDbContext.BatchSize)
                    .Take(ApplicationDbContext.BatchSize);
                // Get the IDs of the items in the current batch.
                var batchIds = batchItems
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id);
                // Get the IDs of the related entities that appear in the current batch.
                var batchDatabaseIds = batchItems
                    .Where(item => item.DatabaseEdges != null)
                    .Select(item => item.DatabaseEdges)
                    .SelectMany(item => item)
                    .Where(item => item.Database != null)
                    .Select(item => item.Database)
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id)
                    .Distinct();
                var batchDatabaseEdgeFieldIds = batchItems
                    .Where(item => item.DatabaseEdgeFieldEdges != null)
                    .Select(item => item.DatabaseEdgeFieldEdges)
                    .SelectMany(item => item)
                    .Where(item => item.DatabaseEdgeField != null)
                    .Select(item => item.DatabaseEdgeField)
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id)
                    .Distinct();
                var batchNodeIds = batchItems
                    .Where(item => item.EdgeNodes != null)
                    .Select(item => item.EdgeNodes)
                    .SelectMany(item => item)
                    .Where(item => item.Node != null)
                    .Select(item => item.Node)
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id)
                    .Distinct();
                // Define the list of items to get.
                var edges = new List<Edge>();
                var databases = new List<Database>();
                var databaseEdgeFields = new List<DatabaseEdgeField>();
                var nodes = new List<Node>();
                // Use a new scope.
                using (var scope = serviceProvider.CreateScope())
                {
                    // Use a new context instance.
                    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    // Get the items with the provided IDs.
                    var items = context.Edges
                        .Where(item => !item.DatabaseEdges.Any(item1 => item1.Database.DatabaseType.Name == "Generic"))
                        .Where(item => batchIds.Contains(item.Id));
                    // Check if there were no items found.
                    if (items == null || !items.Any())
                    {
                        // Continue.
                        continue;
                    }
                    // Get the items found.
                    edges = items
                        .ToList();
                    // Get the related entities that appear in the current batch.
                    databaseEdgeFields = context.DatabaseEdgeFields
                        .Where(item => item.Database.DatabaseType.Name != "Generic")
                        .Where(item => batchDatabaseEdgeFieldIds.Contains(item.Id))
                        .ToList();
                    databases = context.Databases
                        .Where(item => item.DatabaseType.Name != "Generic")
                        .Where(item => batchDatabaseIds.Contains(item.Id))
                        .Concat(context.DatabaseEdgeFields
                            .Where(item => item.Database.DatabaseType.Name != "Generic")
                            .Where(item => batchDatabaseEdgeFieldIds.Contains(item.Id))
                            .Select(item => item.Database))
                        .Distinct()
                        .ToList();
                    nodes = context.Nodes
                        .Where(item => !item.DatabaseNodes.Any(item1 => item1.Database.DatabaseType.Name == "Generic"))
                        .Where(item => batchNodeIds.Contains(item.Id))
                        .ToList();
                }
                // Get the IDs of the items.
                var edgeIds = edges
                    .Select(item => item.Id);
                // Save the items to edit.
                var edgesToEdit = new List<Edge>();
                // Go over each of the valid items.
                foreach (var batchItem in batchItems)
                {
                    // Get the corresponding item.
                    var edge = edges
                        .FirstOrDefault(item => batchItem.Id == item.Id);
                    // Check if there was no item found.
                    if (edge == null)
                    {
                        // Continue.
                        continue;
                    }
                    // Check if there were no edge nodes provided.
                    if (batchItem.EdgeNodes == null || !batchItem.EdgeNodes.Any())
                    {
                        // Throw an exception.
                        throw new TaskException("There were no edge nodes provided.", showExceptionItem, batchItem);
                    }
                    // Get the edge nodes.
                    var edgeNodes = batchItem.EdgeNodes
                        .Where(item => item.Node != null)
                        .Where(item => !string.IsNullOrEmpty(item.Node.Id))
                        .Where(item => item.Type == "Source" || item.Type == "Target")
                        .Select(item => (item.Node.Id, item.Type))
                        .Distinct()
                        .Where(item => nodes.Any(item1 => item1.Id == item.Item1))
                        .Select(item => new EdgeNode
                        {
                            NodeId = item.Item1,
                            Type = EnumerationExtensions.GetEnumerationValue<EdgeNodeType>(item.Item2)
                        });
                    // Check if there were no edge nodes found.
                    if (edgeNodes == null || !edgeNodes.Any(item => item.Type == EdgeNodeType.Source) || !edgeNodes.Any(item => item.Type == EdgeNodeType.Target))
                    {
                        // Throw an exception.
                        throw new TaskException("There were no edge nodes found.", showExceptionItem, batchItem);
                    }
                    // Check if there were no database edges or database edge field edges provided.
                    if ((batchItem.DatabaseEdges == null || !batchItem.DatabaseEdges.Any()) && (batchItem.DatabaseEdgeFieldEdges == null || !batchItem.DatabaseEdgeFieldEdges.Any()))
                    {
                        // Throw an exception.
                        throw new TaskException("There were no database edges or database edge field edges provided.", showExceptionItem, batchItem);
                    }
                    // Get the database edge field edges.
                    var databaseEdgeFieldEdges = batchItem.DatabaseEdgeFieldEdges != null ?
                        batchItem.DatabaseEdgeFieldEdges
                            .Where(item => item.DatabaseEdgeField != null)
                            .Where(item => !string.IsNullOrEmpty(item.DatabaseEdgeField.Id))
                            .Where(item => !string.IsNullOrEmpty(item.Value))
                            .Select(item => (item.DatabaseEdgeField.Id, item.Value))
                            .Distinct()
                            .Where(item => databaseEdgeFields.Any(item1 => item1.Id == item.Item1))
                            .Select(item => new DatabaseEdgeFieldEdge
                            {
                                DatabaseEdgeFieldId = item.Item1,
                                Value = item.Item2
                            }) :
                        Enumerable.Empty<DatabaseEdgeFieldEdge>();
                    // Get the database edges.
                    var databaseEdgeFieldIds = databaseEdgeFieldEdges
                        .Select(item => item.DatabaseEdgeFieldId)
                        .Distinct();
                    var currentDatabaseEdgeFields = databaseEdgeFields
                        .Where(item => databaseEdgeFieldIds.Contains(item.Id));
                    var databaseEdges = batchItem.DatabaseEdges != null ?
                        batchItem.DatabaseEdges
                            .Where(item => item.Database != null)
                            .Where(item => !string.IsNullOrEmpty(item.Database.Id))
                            .Select(item => item.Database.Id)
                            .Concat(currentDatabaseEdgeFields
                                .Select(item => item.Database.Id))
                            .Distinct()
                            .Where(item => databases.Any(item1 => item1.Id == item))
                            .Select(item => new DatabaseEdge
                            {
                                DatabaseId = item,
                            }) :
                        Enumerable.Empty<DatabaseEdge>();
                    // Check if there were no database edges found.
                    if (databaseEdges == null || !databaseEdges.Any())
                    {
                        // Throw an exception.
                        throw new TaskException("There were no database edges found.", showExceptionItem, batchItem);
                    }
                    // Update the edge.
                    edge.Name = string.Concat(nodes.First(item => item.Id == edgeNodes.First(item => item.Type == EdgeNodeType.Source).NodeId).Name, " - ", nodes.First(item => item.Id == edgeNodes.First(item => item.Type == EdgeNodeType.Target).NodeId).Name);
                    edge.Description = batchItem.Description;
                    edge.EdgeNodes = new List<EdgeNode>
                    {
                        edgeNodes.First(item => item.Type == EdgeNodeType.Source),
                        edgeNodes.First(item => item.Type == EdgeNodeType.Target)
                    };
                    edge.DatabaseEdgeFieldEdges = databaseEdgeFieldEdges.ToList();
                    edge.DatabaseEdges = databaseEdges.ToList();
                    // Add the edge to the list.
                    edgesToEdit.Add(edge);
                }
                // Delete the dependent entities.
                await EdgeExtensions.DeleteDependentAnalysesAsync(edgeIds, serviceProvider, token);
                await EdgeExtensions.DeleteDependentNetworksAsync(edgeIds, serviceProvider, token);
                // Delete the related entities.
                await EdgeExtensions.DeleteRelatedEntitiesAsync<EdgeNode>(edgeIds, serviceProvider, token);
                await EdgeExtensions.DeleteRelatedEntitiesAsync<DatabaseEdgeFieldEdge>(edgeIds, serviceProvider, token);
                await EdgeExtensions.DeleteRelatedEntitiesAsync<DatabaseEdge>(edgeIds, serviceProvider, token);
                // Update the items.
                await IEnumerableExtensions.EditAsync(edgesToEdit, serviceProvider, token);
            }
        }

        /// <summary>
        /// Deletes the items from the database.
        /// </summary>
        /// <param name="serviceProvider">The application service provider.</param>
        /// <param name="token">The cancellation token for the task.</param>
        public async Task DeleteAsync(IServiceProvider serviceProvider, CancellationToken token)
        {
            // Check if there weren't any valid items found.
            if (Items == null)
            {
                // Throw an exception.
                throw new TaskException("No valid items could be found with the provided data.");
            }
            // Get the total number of batches.
            var count = Math.Ceiling((double)Items.Count() / ApplicationDbContext.BatchSize);
            // Go over each batch.
            for (var index = 0; index < count; index++)
            {
                // Check if the cancellation was requested.
                if (token.IsCancellationRequested)
                {
                    // Break.
                    break;
                }
                // Get the items in the current batch.
                var batchItems = Items
                    .Skip(index * ApplicationDbContext.BatchSize)
                    .Take(ApplicationDbContext.BatchSize);
                // Get the IDs of the items in the current batch.
                var batchIds = batchItems.Select(item => item.Id);
                // Define the list of items to get.
                var edges = new List<Edge>();
                // Use a new scope.
                using (var scope = serviceProvider.CreateScope())
                {
                    // Use a new context instance.
                    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    // Get the items with the provided IDs.
                    var items = context.Edges
                        .Where(item => batchIds.Contains(item.Id));
                    // Check if there were no items found.
                    if (items == null || !items.Any())
                    {
                        // Continue.
                        continue;
                    }
                    // Get the items found.
                    edges = items
                        .ToList();
                }
                // Get the IDs of the items.
                var edgeIds = edges
                    .Select(item => item.Id);
                // Delete the dependent entities.
                await EdgeExtensions.DeleteDependentAnalysesAsync(edgeIds, serviceProvider, token);
                await EdgeExtensions.DeleteDependentNetworksAsync(edgeIds, serviceProvider, token);
                // Delete the related entities.
                await EdgeExtensions.DeleteRelatedEntitiesAsync<EdgeNode>(edgeIds, serviceProvider, token);
                await EdgeExtensions.DeleteRelatedEntitiesAsync<DatabaseEdgeFieldEdge>(edgeIds, serviceProvider, token);
                await EdgeExtensions.DeleteRelatedEntitiesAsync<DatabaseEdge>(edgeIds, serviceProvider, token);
                // Delete the items.
                await IEnumerableExtensions.DeleteAsync(edges, serviceProvider, token);
            }
        }
    }
}
