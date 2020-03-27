﻿// Copyright (c) 2016 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Test.Chapter03Listings;
using TestSupport.EfHelpers;
using TestSupport.Helpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.TestDataLayer
{
    public class Ch03_SimpleCreate
    {
        private readonly ITestOutputHelper _output;

        public Ch03_SimpleCreate(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestCreateOneEntry()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<SimpleDbContext>();
            using (var context = new SimpleDbContext(options))
            {
                context.Database.EnsureCreated();
                var itemToAdd = new ExampleEntity
                {                               
                    MyMessage = "Hello World"   
                };

                //ATTEMPT
                context.Add(itemToAdd); //#A
                context.SaveChanges(); //#B
                /*********************************************************
                #A It use the Add method to add the SingleEntity to the application's DbContext. The DbContext works what table to add it to based on its type of its parameter
                #B It calls the SaveChanges() method from the application's DbContext to update the database
                 * ***********************************************************/

                //VERIFY
                context.SingleEntities.Count()
                    .ShouldEqual(1);          
                itemToAdd.ExampleEntityId      
                    .ShouldNotEqual(0);       
            }
        }

        [Fact]
        public void TestCreateOneEntryWithLogs()
        {
            //SETUP
            var showLog = false;
            var options = SqliteInMemory.CreateOptionsWithLogging<SimpleDbContext>(log =>
            {
                if (!showLog)
                    _output.WriteLine(log.DecodeMessage());
            });
            using (var context = new SimpleDbContext(options))
            {
                context.Database.EnsureCreated();

                showLog = true;
                var itemToAdd = new ExampleEntity
                {
                    MyMessage = "Hello World"
                };

                //ATTEMPT
                context.SingleEntities.Add(      
                    itemToAdd);                  
                context.SaveChanges();           

                //VERIFY
                context.SingleEntities.Count().ShouldEqual(1);

            }
        }

    }
}