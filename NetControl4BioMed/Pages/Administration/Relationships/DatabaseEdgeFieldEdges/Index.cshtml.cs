using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NetControl4BioMed.Data;
using NetControl4BioMed.Data.Enumerations;
using NetControl4BioMed.Data.Models;
using NetControl4BioMed.Helpers.ViewModels;

namespace NetControl4BioMed.Pages.Administration.Relationships.DatabaseEdgeFieldEdges
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly LinkGenerator _linkGenerator;

        public IndexModel(ApplicationDbContext context, LinkGenerator linkGenerator)
        {
            _context = context;
            _linkGenerator = linkGenerator;
        }

        public ViewModel View { get; set; }

        public class ViewModel
        {
            public SearchViewModel<DatabaseEdgeFieldEdge> Search { get; set; }

            public static SearchOptionsViewModel SearchOptions { get; } = new SearchOptionsViewModel
            {
                SearchIn = new Dictionary<string, string>
                {
                    { "DatabaseEdgeFieldId", "Database edge field ID" },
                    { "DatabaseEdgeFieldName", "Database edge field name" },
                    { "EdgeId", "Edge ID" },
                    { "EdgeName", "Edge name" },
                    { "Value", "Value" }
                },
                Filter = new Dictionary<string, string>
                {
                },
                SortBy = new Dictionary<string, string>
                {
                    { "DatabaseEdgeFieldId", "Database edge field ID" },
                    { "DatabaseEdgeFieldName", "Database edge field name" },
                    { "EdgeId", "Edge ID" },
                    { "EdgeName", "EdgeName" },
                    { "Value", "Value" }
                }
            };
        }

        public IActionResult OnGet(string searchString = null, IEnumerable<string> searchIn = null, IEnumerable<string> filter = null, string sortBy = null, string sortDirection = null, int? itemsPerPage = null, int? currentPage = 1)
        {
            // Define the search input.
            var input = new SearchInputViewModel(ViewModel.SearchOptions, null, searchString, searchIn, filter, sortBy, sortDirection, itemsPerPage, currentPage);
            // Check if any of the provided variables was null before the reassignment.
            if (input.NeedsRedirect)
            {
                // Redirect to the page where they are all explicitly defined.
                return RedirectToPage(new { searchString = input.SearchString, searchIn = input.SearchIn, filter = input.Filter, sortBy = input.SortBy, sortDirection = input.SortDirection, itemsPerPage = input.ItemsPerPage, currentPage = input.CurrentPage });
            }
            // Start with all of the items in the database.
            var query = _context.DatabaseEdgeFieldEdges
                .Where(item => !item.Edge.DatabaseEdges.Any(item1 => item1.Database.DatabaseType.Name == "Generic"));
            // Select the results matching the search string.
            query = query
                .Where(item => !input.SearchIn.Any() ||
                    input.SearchIn.Contains("DatabaseEdgeFieldId") && item.DatabaseEdgeField.Id.Contains(input.SearchString) ||
                    input.SearchIn.Contains("DatabaseEdgeFieldName") && item.DatabaseEdgeField.Name.Contains(input.SearchString) ||
                    input.SearchIn.Contains("EdgeId") && item.Edge.Id.Contains(input.SearchString) ||
                    input.SearchIn.Contains("EdgeName") && item.Edge.Name.Contains(input.SearchString) ||
                    input.SearchIn.Contains("Value") && item.Value.Contains(input.SearchString));
            // Sort it according to the parameters.
            switch ((input.SortBy, input.SortDirection))
            {
                case var sort when sort == ("DatabaseEdgeFieldId", "Ascending"):
                    query = query.OrderBy(item => item.DatabaseEdgeField.Id);
                    break;
                case var sort when sort == ("DatabaseEdgeFieldId", "Descending"):
                    query = query.OrderByDescending(item => item.DatabaseEdgeField.Id);
                    break;
                case var sort when sort == ("DatabaseEdgeFieldName", "Ascending"):
                    query = query.OrderBy(item => item.DatabaseEdgeField.Name);
                    break;
                case var sort when sort == ("DatabaseEdgeFieldName", "Descending"):
                    query = query.OrderByDescending(item => item.DatabaseEdgeField.Name);
                    break;
                case var sort when sort == ("EdgeId", "Ascending"):
                    query = query.OrderBy(item => item.Edge.Id);
                    break;
                case var sort when sort == ("EdgeId", "Descending"):
                    query = query.OrderByDescending(item => item.Edge.Id);
                    break;
                case var sort when sort == ("EdgeName", "Ascending"):
                    query = query.OrderBy(item => item.Edge.Name);
                    break;
                case var sort when sort == ("EdgeName", "Descending"):
                    query = query.OrderByDescending(item => item.Edge.Name);
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
            // Include the related entitites.
            query = query
                .Include(item => item.DatabaseEdgeField)
                .Include(item => item.Edge);
            // Define the view.
            View = new ViewModel
            {
                Search = new SearchViewModel<DatabaseEdgeFieldEdge>(_linkGenerator, HttpContext, input, query)
            };
            // Return the page.
            return Page();
        }
    }
}
