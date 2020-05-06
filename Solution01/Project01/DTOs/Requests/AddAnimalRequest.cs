using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project01.DTOs.Requests
{
    public class AddAnimalRequest
    {
        public string Name { get; set; }
        public string TypeAnimal { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
    }
}
