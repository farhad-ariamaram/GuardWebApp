using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GuardWebApp.Models
{
    public class RunMD
    {
        
    }

    [ModelMetadataType(typeof(RunMD))]
    public partial class Run
    {
    }
}
