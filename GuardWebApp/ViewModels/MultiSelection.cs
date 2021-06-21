using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuardWebApp.ViewModels
{
    public class MultiSelectionVM
    {
        public int[] SelectedIds { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
