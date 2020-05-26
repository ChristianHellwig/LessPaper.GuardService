﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessPaper.GuardService.Models.Database.Dtos;
using LessPaper.Shared.Interfaces.General;

namespace LessPaper.GuardService.Models.Database.Implement
{
    public class MinimalUserInformation : IMinimalUserInformation
    {
        private readonly UserDto dto;

        public MinimalUserInformation(UserDto dto)
        {
            this.dto = dto;
        }

        /// <inheritdoc />
        public string Email => dto.Email;

        /// <inheritdoc />
        public string PasswordHash => dto.PasswordHash;

        /// <inheritdoc />
        public string Salt => dto.Salt;

        /// <inheritdoc />
        public string RootDirectoryId => dto.RootDirectory;

        /// <inheritdoc />
        public string EncryptedPrivateKey => dto.EncryptedPrivateKey;
    }
}
