﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetControl4BioMed.Data;
using NetControl4BioMed.Data.Enumerations;
using NetControl4BioMed.Data.Models;
using NetControl4BioMed.Helpers.Exceptions;
using NetControl4BioMed.Helpers.Extensions;
using NetControl4BioMed.Helpers.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetControl4BioMed.Helpers.Tasks
{
    /// <summary>
    /// Implements a task to update analysis user invitations in the database.
    /// </summary>
    public class AnalysisUserInvitationsTask
    {
        /// <summary>
        /// Gets or sets the items to be updated.
        /// </summary>
        public IEnumerable<AnalysisUserInvitationInputModel> Items { get; set; }

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
                // Get the IDs of the related entities that appear in the current batch.
                var batchAnalysisIds = batchItems
                    .Where(item => item.Analysis != null)
                    .Select(item => item.Analysis)
                    .Where(item => !string.IsNullOrEmpty(item.Id))
                    .Select(item => item.Id)
                    .Distinct();
                var batchUserEmails = batchItems
                    .Where(item => !string.IsNullOrEmpty(item.Email))
                    .Select(item => item.Email)
                    .Distinct();
                // Define the list of items to get.
                var analyses = new List<Analysis>();
                var users = new List<User>();
                // Create a new scope.
                using (var scope = serviceProvider.CreateScope())
                {
                    // Use a new context instance.
                    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    // Get the related entities that appear in the current batch.
                    analyses = context.Analyses
                        .Where(item => batchAnalysisIds.Contains(item.Id))
                        .ToList();
                    users = context.Users
                        .Where(item => batchUserEmails.Contains(item.Email))
                        .ToList();
                }
                // Save the items to add.
                var analysisUserInvitationsToAdd = new List<AnalysisUserInvitation>();
                // Go over each item in the current batch.
                foreach (var batchItem in batchItems)
                {
                    // Check if there was no analysis provided.
                    if (batchItem.Analysis == null || string.IsNullOrEmpty(batchItem.Analysis.Id))
                    {
                        // Throw an exception.
                        throw new TaskException("There was no analysis provided.", showExceptionItem, batchItem);
                    }
                    // Get the analysis.
                    var analysis = analyses
                        .FirstOrDefault(item => item.Id == batchItem.Analysis.Id);
                    // Check if there was no analysis found.
                    if (analysis == null)
                    {
                        // Throw an exception.
                        throw new TaskException("There was no analysis found.", showExceptionItem, batchItem);
                    }
                    // Check if there was no e-mail provided.
                    if (string.IsNullOrEmpty(batchItem.Email))
                    {
                        // Throw an exception.
                        throw new TaskException("There was no e-mail provided.", showExceptionItem, batchItem);
                    }
                    // Try to get the user.
                    var user = users
                        .FirstOrDefault(item => item.Email == batchItem.Email);
                    // Check if there was a user found.
                    if (user != null)
                    {
                        // Throw an exception.
                        throw new TaskException("The user with the provided e-mail already exists.", showExceptionItem, batchItem);
                    }
                    // Define the new item.
                    var analysisUserInvitation = new AnalysisUserInvitation
                    {
                        DateTimeCreated = DateTime.UtcNow,
                        AnalysisId = analysis.Id,
                        Email = batchItem.Email
                    };
                    // Add the item to the list.
                    analysisUserInvitationsToAdd.Add(analysisUserInvitation);
                }
                // Create the items.
                await IEnumerableExtensions.CreateAsync(analysisUserInvitationsToAdd, serviceProvider, token);
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
                var batchIds = batchItems
                    .Where(item => item.Analysis != null && !string.IsNullOrEmpty(item.Analysis.Id))
                    .Where(item => !string.IsNullOrEmpty(item.Email))
                    .Select(item => (item.Analysis.Id, item.Email));
                // Get the IDs of all individual items.
                var batchAnalysisIds = batchIds
                    .Select(item => item.Item1);
                var batchEmails = batchIds
                    .Select(item => item.Item2);
                // Define the list of items to get.
                var analysisUserInvitations = new List<AnalysisUserInvitation>();
                // Use a new scope.
                using (var scope = serviceProvider.CreateScope())
                {
                    // Use a new context instance.
                    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    // Get the items with the provided IDs.
                    var items = context.AnalysisUserInvitations
                        .Include(item => item.Analysis)
                        .Where(item => batchAnalysisIds.Contains(item.Analysis.Id))
                        .Where(item => batchEmails.Contains(item.Email))
                        .AsEnumerable()
                        .Where(item => batchIds.Any(item1 => item1.Item1 == item.Analysis.Id && item1.Item2 == item.Email))
                        .ToList();
                    // Check if there were no items found.
                    if (items == null || !items.Any())
                    {
                        // Continue.
                        continue;
                    }
                    // Get the items found.
                    analysisUserInvitations = items
                        .ToList();
                }
                // Delete the items.
                await IEnumerableExtensions.DeleteAsync(analysisUserInvitations, serviceProvider, token);
            }
        }
    }
}
