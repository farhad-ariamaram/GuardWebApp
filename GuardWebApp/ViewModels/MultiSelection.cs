using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GuardWebApp.ViewModels
{
    public class MultiSelectionVM
    {
        public long[] SelectedIds { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
