using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetControl4BioMed.Data;
using NetControl4BioMed.Data.Models;
using NetControl4BioMed.Helpers.ViewModels;

namespace NetControl4BioMed.Pages.Content.Relationships.DatabaseNodeFieldNodes
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly LinkGenerator _linkGenerator;

        public IndexModel(UserManager<User> userManager, ApplicationDbContext context, LinkGenerator linkGenerator)
        {
            _userManager = userManager;
            _context = context;
            _linkGenerator = linkGenerator;
        }

        public ViewModel View { get; set; }

        public class ViewModel
        {
            public SearchViewModel<ItemModel> Search { get; set; }

            public static SearchOptionsViewModel SearchOptions { get; } = new SearchOptionsViewModel
            {
                SearchIn = new Dictionary<string, string>
                {
                    { "DatabaseNodeFieldId", "Database node field ID" },
                    { "DatabaseNodeFieldName", "Database node field name" },
                    { "NodeId", "Node ID" },
                    { "NodeName", "Node name" },
                    { "Value", "Value" }
                },
                Filter = new Dictionary<string, string>
                {
                },
                SortBy = new Dictionary<string, string>
                {
                    { "DatabaseNodeFieldId", "Database node field ID" },
                    { "DatabaseNodeFieldName", "Database node field name" },
                    { "NodeId", "Node ID" },
                    { "NodeName", "NodeName" },
                    { "Value", "Value" }
                }
            };
        }

        public class ItemModel
        {
            public string DatabaseNodeFieldId { get; set; }

            public string DatabaseNodeFieldName { get; set; }

            public string NodeId { get; set; }

            public string NodeName { get; set; }

            public string Value { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string searchString = null, IEnumerable<string> searchIn = null, IEnumerable<string> filter = null, string sortBy = null, string sortDirection = null, int? itemsPerPage = null, int? currentPage = 1)
        {
            // Get the current user.
            var user = await _userManager.GetUserAsync(User);
            // Define the search input.
            var input = new SearchInputViewModel(ViewModel.SearchOptions, null, searchString, searchIn, filter, sortBy, sortDirection, itemsPerPage, currentPage);
            // Check if any of the provided variables was null before the reassignment.
            if (input.NeedsRedirect)
            {
                // Redirect to the page where they are all explicitly defined.
                return RedirectToPage(new { searchString = input.SearchString, searchIn = input.SearchIn, filter = input.Filter, sortBy = input.SortBy, sortDirection = input.SortDirection, itemsPerPage = input.ItemsPerPage, currentPage = input.CurrentPage });
            }
            // Start with all of the items in the non-generic databases.
            var query = _context.DatabaseNodeFieldNodes
                .Where(item => item.DatabaseNodeField.Database.DatabaseType.Name != "Generic" && (item.DatabaseNodeField.Database.IsPublic || item.DatabaseNodeField.Database.DatabaseUsers.Any(item1 => item1.User == user)))
                .Where(item => !item.Node.DatabaseNodes.Any(item1 => item1.Database.DatabaseType.Name == "Generic") && item.Node.DatabaseNodes.Any(item1 => item1.Database.IsPublic || item1.Database.DatabaseUsers.Any(item2 => item2.User == user)));
            // Select the results matching the search string.
            query = query
                .Where(item => !input.SearchIn.Any() ||
                    input.SearchIn.Contains("DatabaseNodeFieldId") && item.DatabaseNodeField.Id.Contains(input.SearchString) ||
                    input.SearchIn.Contains("DatabaseNodeFieldName") && item.DatabaseNodeField.Name.Contains(input.SearchString) ||
                    input.SearchIn.Contains("NodeId") && item.Node.Id.Contains(input.SearchString) ||
                    input.SearchIn.Contains("NodeName") && item.Node.Name.Contains(input.SearchString) ||
                    input.SearchIn.Contains("Value") && item.Value.Contains(input.SearchString));
            // Sort it according to the parameters.
            switch ((input.SortBy, input.SortDirection))
            {
                case var sort when sort == ("DatabaseNodeFieldId", "Ascending"):
                    query = query.OrderBy(item => item.DatabaseNodeField.Id);
                    break;
                case var sort when sort == ("DatabaseNodeFieldId", "Descending"):
                    query = query.OrderByDescending(item => item.DatabaseNodeField.Id);
                    break;
                case var sort when sort == ("DatabaseNodeFieldName", "Ascending"):
                    query = query.OrderBy(item => item.DatabaseNodeField.Name);
                    break;
                case var sort when sort == ("DatabaseNodeFieldName", "Descending"):
                    query = query.OrderByDescending(item => item.DatabaseNodeField.Name);
                    break;
                case var sort when sort == ("NodeId", "Ascending"):
                    query = query.OrderBy(item => item.Node.Id);
                    break;
                case var sort when sort == ("NodeId", "Descending"):
                    query = query.OrderByDescending(item => item.Node.Id);
                    break;
                case var sort when sort == ("NodeName", "Ascending"):
                    query = query.OrderBy(item => item.Node.Name);
                    break;
                case var sort when sort == ("NodeName", "Descending"):
                    query = query.OrderByDescending(item => item.Node.Name);
                    break;
                case var sort when sort == ("Value", "Ascending"):
                    query = query.OrderBy(item => item.Value);
                    break;
                case var sort when sort == ("Value", "Descending"):
                    query = query.OrderByDescending(item => item.Value);
                    break;
                default:
                    break;
            }
            // Define the view.
            View = new ViewModel
            {
                Search = new SearchViewModel<ItemModel>(_linkGenerator, HttpContext, input, query.Select(item => new ItemModel
                {
                    DatabaseNodeFieldId = item.DatabaseNodeField.Id,
                    DatabaseNodeFieldName = item.DatabaseNodeField.Name,
                    NodeId = item.Node.Id,
                    NodeName = item.Node.Name,
                    Value = item.Value
                }))
            };
            // Return the page.
            return Page();
        }
    }
}