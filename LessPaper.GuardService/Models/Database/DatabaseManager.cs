﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LessPaper.GuardService.Models.Database.Dtos;
using LessPaper.Shared.Enums;
using LessPaper.Shared.Interfaces.Database.Manager;
using Microsoft.AspNetCore.Routing.Template;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace LessPaper.GuardService.Models.Database
{
    public class DatabaseManager : IDbManager
    {
        public DatabaseManager()
        {
            //var cs = "mongodb://user1:masterkey@127.0.0.1:27017?retryWrites=false";
            var cs = "mongodb://192.168.0.227:28017?retryWrites=false";


            var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(cs));
            mongoClientSettings.RetryWrites = false;
            //mongoClientSettings.ReadPreference = ReadPreference.PrimaryPreferred;

            mongoClientSettings.ClusterConfigurator = cb => {
                cb.Subscribe<CommandStartedEvent>(e => {
                    Trace.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };
            var dbClient = new MongoClient(mongoClientSettings);


            var db = dbClient.GetDatabase("lesspaper");
            

            var userCollection = db.GetCollection<UserDto>("user");
            var directoryCollection = db.GetCollection<DirectoryDto>("directories");


            var uniqueEmail = new CreateIndexModel<UserDto>(
                Builders<UserDto>.IndexKeys.Ascending(x => x.Email),
                new CreateIndexOptions { Unique = true });
            
            userCollection.Indexes.CreateOne(uniqueEmail);

            var uniqueDirectoryName = new CreateIndexModel<DirectoryDto>(
                Builders<DirectoryDto>.IndexKeys.Combine(
                    Builders<DirectoryDto>.IndexKeys.Ascending(x => x.ObjectName),
                    Builders<DirectoryDto>.IndexKeys.Ascending(x => x.Owner)
                ),
                new CreateIndexOptions { Unique = true });

            directoryCollection.Indexes.CreateOne(uniqueDirectoryName);

            DbFileManager = new DbFileManager(dbClient, directoryCollection, userCollection);
            DbDirectoryManager = new DbDirectoryManager(dbClient, directoryCollection);
            DbUserManager = new DbUserManager(dbClient, userCollection, directoryCollection);
            DbSearchManager = new DbSearchManager(db);
        }

        /// <inheritdoc />
        public IDbFileManager DbFileManager { get; }

        /// <inheritdoc />
        public IDbDirectoryManager DbDirectoryManager { get; }

        /// <inheritdoc />
        public IDbUserManager DbUserManager { get; }

        /// <inheritdoc />
        public IDbSearchManager DbSearchManager { get; }
    }
}
