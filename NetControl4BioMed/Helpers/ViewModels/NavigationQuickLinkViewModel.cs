﻿using NetControl4BioMed.Data.Enumerations;
using NetControl4BioMed.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetControl4BioMed.Helpers.ViewModels
{
    /// <summary>
    /// Represents a navigation quick link.
    /// </summary>
    public class NavigationQuickLinkViewModel
    {
        /// <summary>
        /// Gets or sets the ID of the quick link.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title (displayed text) of the quick link.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the icon of the banner.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the color of the banner.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the width of the quick link.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the number of items of the quick link.
        /// </summary>
        public int? ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the route ID of the quick link.
        /// </summary>
        public string RouteId { get; set; }

        /// <summary>
        /// Gets or sets the destination link of the quick link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets the users navigation quick link for the administration index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AdministrationUsersNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Users",
            Title = "User",
            Color = "success",
            Icon = "fa-user",
            Width = 6,
            Link = "/Administration/Accounts/Users/Index"
        };

        /// <summary>
        /// Gets the roles navigation quick link for the administration index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AdministrationRolesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Roles",
            Title = "Role",
            Color = "success",
            Icon = "fa-tag",
            Width = 6,
            Link = "/Administration/Accounts/Roles/Index"
        };

        /// <summary>
        /// Gets the databases navigation quick link for the administration index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AdministrationDatabasesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Databases",
            Title = "Database",
            Color = "info",
            Icon = "fa-database",
            Width = 6,
            Link = "/Administration/Databases/Databases/Index"
        };

        /// <summary>
        /// Gets the node collections navigation quick link for the administration index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AdministrationNodeCollectionsNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "NodeCollections",
            Title = "Node collection",
            Color = "info",
            Icon = "fa-folder",
            Width = 6,
            Link = "/Administration/Data/NodeCollections/Index"
        };

        /// <summary>
        /// Gets the nodes navigation quick link for the administration index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AdministrationNodesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Nodes",
            Title = "Node",
            Color = "primary",
            Icon = "fa-circle",
            Width = 6,
            Link = "/Administration/Data/Nodes/Index"
        };

        /// <summary>
        /// Gets the edges navigation quick link for the administration index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AdministrationEdgesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Edges",
            Title = "Edge",
            Color = "primary",
            Icon = "fa-arrow-right",
            Width = 6,
            Link = "/Administration/Data/Edges/Index"
        };

        /// <summary>
        /// Gets the nodes navigation quick link for the administration index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AdministrationNetworksNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Networks",
            Title = "Network",
            Color = "secondary",
            Icon = "fa-share-alt",
            Width = 6,
            Link = "/Administration/Created/Networks/Index"
        };

        /// <summary>
        /// Gets the edges navigation quick link for the administration index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AdministrationAnalysesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Analyses",
            Title = "Analysis",
            Color = "secondary",
            Icon = "fa-desktop",
            Width = 6,
            Link = "/Administration/Created/Analyses/Index"
        };

        /// <summary>
        /// Gets the networks navigation quick link for the content index page.
        /// </summary>
        public static NavigationQuickLinkViewModel ContentNetworksNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Networks",
            Title = "Network",
            Color = "success",
            Icon = "fa-share-alt",
            Width = 6,
            Link = "/Content/Created/Networks/Index"
        };

        /// <summary>
        /// Gets the analyses navigation quick link for the content index page.
        /// </summary>
        public static NavigationQuickLinkViewModel ContentAnalysesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Analyses",
            Title = "Analysis",
            Color = "success",
            Icon = "fa-desktop",
            Width = 6,
            Link = "/Content/Created/Analyses/Index"
        };

        /// <summary>
        /// Gets the nodes navigation quick link for the networks index page.
        /// </summary>
        public static NavigationQuickLinkViewModel NetworkNodesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Nodes",
            Title = "Node",
            Color = "success",
            Icon = "fa-circle",
            Width = 6,
            Link = "/Content/Created/Networks/Details/Data/Nodes/Index"
        };

        /// <summary>
        /// Gets the edges navigation quick link for the networks index page.
        /// </summary>
        public static NavigationQuickLinkViewModel NetworkEdgesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Edges",
            Title = "Edge",
            Color = "success",
            Icon = "fa-arrow-right",
            Width = 6,
            Link = "/Content/Created/Networks/Details/Data/Edges/Index"
        };

        /// <summary>
        /// Gets the databases navigation quick link for the networks index page.
        /// </summary>
        public static NavigationQuickLinkViewModel NetworkDatabasesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Databases",
            Title = "Database",
            Color = "info",
            Icon = "fa-database",
            Width = 6,
            Link = "/Content/Created/Networks/Details/Databases/Databases/Index"
        };

        /// <summary>
        /// Gets the node collections navigation quick link for the networks index page.
        /// </summary>
        public static NavigationQuickLinkViewModel NetworkNodeCollectionsNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "NodeCollections",
            Title = "Node collection",
            Color = "info",
            Icon = "fa-folder",
            Width = 6,
            Link = "/Content/Created/Networks/Details/Data/NodeCollections/Index"
        };

        /// <summary>
        /// Gets the users navigation quick link for the networks index page.
        /// </summary>
        public static NavigationQuickLinkViewModel NetworkUsersNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Users",
            Title = "User",
            Color = "primary",
            Icon = "fa-user",
            Width = 6,
            Link = "/Content/Created/Networks/Details/Accounts/Users/Index"
        };

        /// <summary>
        /// Gets the analyses navigation quick link for the networks index page.
        /// </summary>
        public static NavigationQuickLinkViewModel NetworkAnalysesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Analyses",
            Title = "Analysis",
            Color = "primary",
            Icon = "fa-desktop",
            Width = 6,
            Link = "/Content/Created/Networks/Details/Created/Analyses/Index"
        };

        /// <summary>
        /// Gets the nodes navigation quick link for the analysis index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AnalysisNodesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Nodes",
            Title = "Node",
            Color = "success",
            Icon = "fa-circle",
            Width = 6,
            Link = "/Content/Created/Analyses/Details/Data/Nodes/Index"
        };

        /// <summary>
        /// Gets the edges navigation quick link for the analysis index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AnalysisEdgesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Edges",
            Title = "Edge",
            Color = "success",
            Icon = "fa-arrow-right",
            Width = 6,
            Link = "/Content/Created/Analyses/Details/Data/Edges/Index"
        };

        /// <summary>
        /// Gets the databases navigation quick link for the analysis index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AnalysisDatabasesNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Databases",
            Title = "Database",
            Color = "info",
            Icon = "fa-database",
            Width = 6,
            Link = "/Content/Created/Analyses/Details/Databases/Databases/Index"
        };

        /// <summary>
        /// Gets the node collections navigation quick link for the analysis index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AnalysisNodeCollectionsNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "NodeCollections",
            Title = "Node collection",
            Color = "info",
            Icon = "fa-folder",
            Width = 6,
            Link = "/Content/Created/Analyses/Details/Data/NodeCollections/Index"
        };

        /// <summary>
        /// Gets the users navigation quick link for the analysis index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AnalysisUsersNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Users",
            Title = "User",
            Color = "primary",
            Icon = "fa-user",
            Width = 6,
            Link = "/Content/Created/Analyses/Details/Accounts/Users/Index"
        };

        /// <summary>
        /// Gets the networks navigation quick link for the analysis index page.
        /// </summary>
        public static NavigationQuickLinkViewModel AnalysisNetworksNavigationQuickLink { get; } = new NavigationQuickLinkViewModel
        {
            Id = "Networks",
            Title = "Network",
            Color = "primary",
            Icon = "fa-share-alt",
            Width = 6,
            Link = "/Content/Created/Analyses/Details/Created/Networks/Index"
        };

        /// <summary>
        /// Gets the navigation quick links for the administration index page.
        /// </summary>
        /// <param name="count">The dictionary containing the current counts.</param>
        /// <returns>The navigation quick links for the content index page.</returns>
        public static IEnumerable<NavigationQuickLinkViewModel> GetAdministrationNavigationQuickLinks(Dictionary<string, int?> count = null)
        {
            // Get the corresponding navigation quick links.
            var administrationUsersNavigationQuickLink = AdministrationUsersNavigationQuickLink;
            var administrationRolesNavigationQuickLink = AdministrationRolesNavigationQuickLink;
            var administrationDatabasesNavigationQuickLink = AdministrationDatabasesNavigationQuickLink;
            var administrationNodeCollectionsNavigationQuickLink = AdministrationNodeCollectionsNavigationQuickLink;
            var administrationNodesNavigationQuickLink = AdministrationNodesNavigationQuickLink;
            var administrationEdgesNavigationQuickLink = AdministrationEdgesNavigationQuickLink;
            var administrationNetworksNavigationQuickLink = AdministrationNetworksNavigationQuickLink;
            var administrationAnalysesNavigationQuickLink = AdministrationAnalysesNavigationQuickLink;
            // Update the count and the route ID.
            administrationUsersNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Users", null);
            administrationRolesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Roles", null);
            administrationDatabasesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Databases", null);
            administrationNodeCollectionsNavigationQuickLink.ItemCount = count?.GetValueOrDefault("NodeCollections", null);
            administrationNodesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Nodes", null);
            administrationEdgesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Edges", null);
            administrationNetworksNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Networks", null);
            administrationAnalysesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Analyses", null);
            // Return the navigation quick links.
            return new List<NavigationQuickLinkViewModel>
            {
                administrationUsersNavigationQuickLink,
                administrationRolesNavigationQuickLink,
                administrationDatabasesNavigationQuickLink,
                administrationNodeCollectionsNavigationQuickLink,
                administrationNodesNavigationQuickLink,
                administrationEdgesNavigationQuickLink,
                administrationNetworksNavigationQuickLink,
                administrationAnalysesNavigationQuickLink
            };
        }

        /// <summary>
        /// Gets the navigation quick links for the content index page.
        /// </summary>
        /// <param name="count">The dictionary containing the current counts.</param>
        /// <returns>The navigation quick links for the content index page.</returns>
        public static IEnumerable<NavigationQuickLinkViewModel> GetContentNavigationQuickLinks(Dictionary<string, int?> count = null)
        {
            // Get the corresponding navigation quick links.
            var contentNetworksNavigationQuickLink = ContentNetworksNavigationQuickLink;
            var contentAnalysesNavigationQuickLink = ContentAnalysesNavigationQuickLink;
            // Update the count and the route ID.
            contentNetworksNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Networks", null);
            contentAnalysesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Analyses", null);
            // Return the navigation quick links.
            return new List<NavigationQuickLinkViewModel>
            {
                contentNetworksNavigationQuickLink,
                contentAnalysesNavigationQuickLink
            };
        }

        /// <summary>
        /// Gets the navigation quick links for the network index page.
        /// </summary>
        /// <param name="networkId">The ID of the current network.</param>
        /// <param name="count">The dictionary containing the current counts.</param>
        /// <returns>The navigation quick links for the network index page.</returns>
        public static IEnumerable<NavigationQuickLinkViewModel> GetNetworkNavigationQuickLinks(string networkId = null, Dictionary<string, int?> count = null)
        {
            // Check if there is no network ID provided.
            if (string.IsNullOrEmpty(networkId))
            {
                // Assign the empty string to it.
                networkId = string.Empty;
            }
            // Get the corresponding navigation quick links.
            var networkNodesNavigationQuickLink = NetworkNodesNavigationQuickLink;
            var networkEdgesNavigationQuickLink = NetworkEdgesNavigationQuickLink;
            var networkDatabasesNavigationQuickLink = NetworkDatabasesNavigationQuickLink;
            var networkNodeCollectionsNavigationQuickLink = NetworkNodeCollectionsNavigationQuickLink;
            var networkUsersNavigationQuickLink = NetworkUsersNavigationQuickLink;
            var networkAnalysesNavigationQuickLink = NetworkAnalysesNavigationQuickLink;
            // Update the count and the route ID.
            networkNodesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Nodes", null);
            networkNodesNavigationQuickLink.RouteId = networkId;
            networkEdgesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Edges", null);
            networkEdgesNavigationQuickLink.RouteId = networkId;
            networkDatabasesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Databases", null);
            networkDatabasesNavigationQuickLink.RouteId = networkId;
            networkNodeCollectionsNavigationQuickLink.ItemCount = count?.GetValueOrDefault("NodeCollections", null);
            networkNodeCollectionsNavigationQuickLink.RouteId = networkId;
            networkUsersNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Users", null);
            networkUsersNavigationQuickLink.RouteId = networkId;
            networkAnalysesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Analyses", null);
            networkAnalysesNavigationQuickLink.RouteId = networkId;
            // Return the navigation quick links.
            return new List<NavigationQuickLinkViewModel>
            {
                networkNodesNavigationQuickLink,
                networkEdgesNavigationQuickLink,
                networkDatabasesNavigationQuickLink,
                networkNodeCollectionsNavigationQuickLink,
                networkUsersNavigationQuickLink,
                networkAnalysesNavigationQuickLink
            };
        }

        /// <summary>
        /// Gets the navigation quick links for the analysis index page.
        /// </summary>
        /// <param name="analysisId">The ID of the current analysis.</param>
        /// <param name="count">The dictionary containing the current counts.</param>
        /// <returns>The navigation quick links for the analysis index page.</returns>
        public static IEnumerable<NavigationQuickLinkViewModel> GetAnalysisNavigationQuickLinks(string analysisId = null, Dictionary<string, int?> count = null)
        {
            // Check if there is no analysis ID provided.
            if (string.IsNullOrEmpty(analysisId))
            {
                // Assign the empty string to it.
                analysisId = string.Empty;
            }
            // Get the corresponding navigation quick links.
            var analysisNodesNavigationQuickLink = AnalysisNodesNavigationQuickLink;
            var analysisEdgesNavigationQuickLink = AnalysisEdgesNavigationQuickLink;
            var analysisDatabasesNavigationQuickLink = AnalysisDatabasesNavigationQuickLink;
            var analysisNodeCollectionsNavigationQuickLink = AnalysisNodeCollectionsNavigationQuickLink;
            var analysisUsersNavigationQuickLink = AnalysisUsersNavigationQuickLink;
            var analysisNetworksNavigationQuickLink = AnalysisNetworksNavigationQuickLink;
            // Update the count and the route ID.
            analysisNodesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Nodes", null);
            analysisNodesNavigationQuickLink.RouteId = analysisId;
            analysisEdgesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Edges", null);
            analysisEdgesNavigationQuickLink.RouteId = analysisId;
            analysisDatabasesNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Databases", null);
            analysisDatabasesNavigationQuickLink.RouteId = analysisId;
            analysisNodeCollectionsNavigationQuickLink.ItemCount = count?.GetValueOrDefault("NodeCollections", null);
            analysisNodeCollectionsNavigationQuickLink.RouteId = analysisId;
            analysisUsersNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Users", null);
            analysisUsersNavigationQuickLink.RouteId = analysisId;
            analysisNetworksNavigationQuickLink.ItemCount = count?.GetValueOrDefault("Networks", null);
            analysisNetworksNavigationQuickLink.RouteId = analysisId;
            // Return the navigation quick links.
            return new List<NavigationQuickLinkViewModel>
            {
                analysisNodesNavigationQuickLink,
                analysisEdgesNavigationQuickLink,
                analysisDatabasesNavigationQuickLink,
                analysisNodeCollectionsNavigationQuickLink,
                analysisUsersNavigationQuickLink,
                analysisNetworksNavigationQuickLink
            };
        }
    }
}
