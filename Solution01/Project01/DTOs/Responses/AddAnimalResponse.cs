using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project01.DTOs.Responses
{
    public class AddAnimalResponse
    {
        public string Name { get; set; }
        public string TypeAnimal { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string OwnerFullName { get; set; }
    }
}
