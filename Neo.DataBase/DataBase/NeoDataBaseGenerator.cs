﻿using Neo.DataBaseHelpers.Configurations;
using Neo.DataBaseHelpers.Extensions;
using Neo.DataBaseHelpers.Vadations;
using Neo.DataBaseRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo.DataBaseDataBase
{
    public static class NeoDataBaseGenerator
    {
        public static async Task GenerateAsync()
        {
            Validation.ValidateConnection();
            var entitys = await GetEntityNamesAsync();
            await NeoDataBaseConfiguration.LocalDataRepository.CheckAndCreateDirectoryAsync();
            entitys.ToList().ForEach(async (entity) => { await (entity + FileFormat.Json).CheckAndCreateFileAsync(NeoDataBaseConfiguration.LocalDataRepository); });

        }

        public static void Generate()
        {
            Validation.ValidateConnection();
            var entitys = GetEntityNames();
            NeoDataBaseConfiguration.LocalDataRepository.CheckAndCreateDirectory();
            entitys.ToList().ForEach((entity) => { (entity + FileFormat.Json).CheckAndCreateFile(NeoDataBaseConfiguration.LocalDataRepository); });
        }

        private static async Task<IEnumerable<string>> GetEntityNamesAsync() => await Task.FromResult(GetEntity());

        private static IEnumerable<string> GetEntityNames() => GetEntity();

        private static IEnumerable<string> GetEntity() => 
            AppDomain.CurrentDomain.GetAssemblies()
                                  .SelectMany(assembly => assembly.GetTypes())
                                  .Where(type => type.IsSubclassOf(typeof(NeoEntity)))
                                  .Select(p => p.Name);
    }
}
