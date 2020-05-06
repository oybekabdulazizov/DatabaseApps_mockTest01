using Project01.DTOs.Requests;
using Project01.DTOs.Responses;
using Project01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project01.Services
{
    public interface IDbService
    { 
        public IEnumerable<Animal> GetAnimals(string orderBy, string inOrder);
        public AddAnimalResponse AddAnimal(AddAnimalRequest request);
        public void SaveLogData(string requester, string path, bool permissionGranted, 
                                        string method, string queryString, string bodyString);
    }
}
