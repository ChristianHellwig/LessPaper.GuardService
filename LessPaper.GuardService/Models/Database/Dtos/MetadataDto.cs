﻿using System;
using System.Collections.Generic;
using System.Linq;
using LessPaper.Shared.Enums;
using LessPaper.Shared.Interfaces.General;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;

namespace LessPaper.GuardService.Models.Database.Dtos
{
    public class MetadataDto : BaseDto
    {
        public string ObjectName { get; set; }
        
        public string OwnerId { get; set; }

        public BasicPermissionDto[] Permissions { get; set; }

        public string[] PathIds { get; set; }

        public string ParentDirectoryId { get; set; }
    }
}
