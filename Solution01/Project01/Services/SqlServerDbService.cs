﻿using Project01.DTOs.Requests;
using Project01.DTOs.Responses;
using Project01.Helpers;
using Project01.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace Project01.Services
{
    public class SqlServerDbService : IDbService
    {
        public AddAnimalResponse AddAnimal(AddAnimalRequest request)
        {
            int idOwner = 0;
            using (var connection = new SqlConnection(LocalDatabase.connectionString))
            {
                using (var command = new SqlCommand())
                {
                    if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.TypeAnimal) ||
                        string.IsNullOrEmpty(request.AdmissionDate.ToString()) ||
                        string.IsNullOrEmpty(request.OwnerFirstName) || string.IsNullOrEmpty(request.OwnerLastName))
                    {
                        return null;
                        // throw new MyException(MyException.ExceptionType.NotFound, "Please enter all the values!");
                    }

                    command.Connection = connection;
                    connection.Open();
                    var transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    string commandText = "INSERT INTO Owners VALUES (@fname, @lname)";
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("fname", request.OwnerFirstName);
                    command.Parameters.AddWithValue("lname", request.OwnerLastName);
                    command.ExecuteNonQuery();

                    commandText = @"SELECT MAX(IdOwner) as LastIndex FROM Owners;";
                    command.CommandText = commandText;
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!string.IsNullOrEmpty(dr["LastIndex"].ToString()))
                            {
                                idOwner = int.Parse(dr["LastIndex"].ToString());
                            }
                            else
                            {
                                idOwner++;
                            }
                        }
                    }

                    commandText = @"INSERT INTO Animals Values (@name, @type, @admissionDate, @idOwner);";
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("name", request.Name);
                    command.Parameters.AddWithValue("type", request.TypeAnimal);
                    command.Parameters.AddWithValue("admissionDate", request.AdmissionDate);
                    command.Parameters.AddWithValue("idOwner", idOwner);
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    return new AddAnimalResponse
                    {
                        Name = request.Name,
                        TypeAnimal = request.TypeAnimal,
                        AdmissionDate = request.AdmissionDate,
                        OwnerFullName = request.OwnerFirstName + " " + request.OwnerLastName
                    };
                }
            }
        }

        public IEnumerable<Animal> GetAnimals(string orderBy, string inOrder)
        {
            var animals = new List<Animal>();
            var orderBys = new List<string> { "name", "admissiondate", "lastname", "typeanimal" };
            using (var connection = new SqlConnection(LocalDatabase.connectionString))
            {
                using (var command = new SqlCommand())
                {
                    if (!orderBys.Contains(orderBy.ToLower()))
                    {
                        throw new MyException(MyException.ExceptionType.BadRequest, "Please provide valid parameters!");
                    }

                    command.Connection = connection;
                    command.CommandText = $@"SELECT a.Name, a.TypeAnimal, a.AdmissionDate, o.LastName 
                                            FROM Animals a JOIN Owners o ON a.IdOwner=o.IdOwner 
                                            ORDER BY {orderBy.ToLower()} {inOrder.ToLower()}";
                    connection.Open();
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            animals.Add(new Animal
                            {
                                Name = dr["Name"].ToString(),
                                TypeAnimal = dr["TypeAnimal"].ToString(),
                                AdmissionDate = DateTime.Parse(dr["AdmissionDate"].ToString()),
                                OwnerLastName = dr["LastName"].ToString()
                            });
                        }
                    }

                    return animals;
                }
            }
        }

        public void SaveLogData(string requester, string path, bool permissionGranted, 
                                        string method, string queryString, string bodyString)
        {
            try
            {
                string logPath = @"log.txt";
                using (StreamWriter writer = new StreamWriter(logPath))
                {
                    writer.WriteLine("Requester: " + requester);
                    writer.WriteLine("Path: " + path);
                    writer.WriteLine("PermissionGranted: " + permissionGranted);
                    writer.WriteLine("Method: " + method);
                    writer.WriteLine("QueryString: " + queryString);
                    writer.WriteLine("Body: " + bodyString);
                    writer.WriteLine("***************************");
                    writer.Flush();
                }
            }
            catch (IOException ex) 
            {
                throw new IOException();
            }
        }
    }
}
