using Application.Dtos;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Infraestructure.Application.AppServices;
using Infraestructure.Application.Validator;
using Infraestructure.Domain.Repositories;
using Infraestructure.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.AppServices
{
    [TestFixture]
    public class AppServices
    {


        #region Data Set
        private static Guid gatewayId = Guid.NewGuid();

        //private static readonly object[] GatewaysWithValidationFails =
        //{
        //    new object[]{   new GatewayDto{Id = new Guid("c48c2a37-4388-4fa0-96d8-f97a61cbef74"),Name="Name 1",Ipv4Address="4999.168.10.1" } ,
        //                    new Gateway{Id = new Guid("c48c2a37-4388-4fa0-96d8-f97a61cbef74"),Name="Name 1",Ipv4Address="4999.168.10.1" }},
        //    new object[]{   new GatewayDto{Id = new Guid("e36bfda8-54c8-4807-8111-31e7efc8ac18"),Name="Name 1",Ipv4Address="192.4999.10.1" },
        //                    new Gateway{Id = new Guid("e36bfda8-54c8-4807-8111-31e7efc8ac18"),Name="Name 1",Ipv4Address="192.4999.10.1" }},
        //    new object[]{   new GatewayDto{Id = new Guid("a0c892c8-0bcd-472b-9380-f9a972f65d15"),Name="Name 1",Ipv4Address="192.168.10.1",
        //                        Peripherals=new List<PeripheralDto>() {
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                    } },
        //                    new Gateway{Id = new Guid("a0c892c8-0bcd-472b-9380-f9a972f65d15"),Name="Name 1",Ipv4Address="192.168.10.1",
        //                        Peripherals=new List<Peripheral>() {
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                    } }
        //    }
        //};

        //private static readonly object[] GatewaysWithValidationSuccess =
        //{
        //                    new Gateway{Id = new Guid("c48c2a37-4388-4fa0-96d8-f97a61cbef74"),Name="Name 1",Ipv4Address="192.168.10.1" },

        //                    new Gateway{Id = new Guid("e36bfda8-54c8-4807-8111-31e7efc8ac18"),Name="Name 1",Ipv4Address="192.168.10.1" },

        //                    new Gateway{Id = new Guid("a0c892c8-0bcd-472b-9380-f9a972f65d15"),Name="Name 1",Ipv4Address="192.168.10.1",
        //                        Peripherals=new List<Peripheral>() {
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //                        new Peripheral{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId}
        //                    } }
            
        //}; 
        #endregion



        //[Test]
        //[TestCaseSource(nameof(GatewaysWithValidationFails))]
        //public void AddGatewaysThrowExceptionOnValidationFails(GatewayDto itemDtoVersion, Gateway entityVersion )
        //{
        //    var options = DataBaseService.DataBaseProviderSetUp.SQLServerInMemoryOptions("database");
        //    var context = new UnitOfWorkContainer(options);
        //    var gatewayRepository = new CaoFaturaRepository(context);
        //    var mockedMapper = new Mock<IMapper>();
        //    mockedMapper.Setup(x => x.Map<Gateway>(itemDtoVersion)).Returns(entityVersion);
        //    var gatewayService = new GatewayAppService(gatewayRepository, mockedMapper.Object, new DataAnnotationsEntityValidator());

        //    Action action = () => gatewayService.AddAsync(itemDtoVersion).Wait();
            
        //    action.Should().Throw<ApplicationValidationErrorsException>();
        //}


        //[Test]
        //[TestCaseSource(nameof(GatewaysWithValidationSuccess))]
        //public async Task AddGatewaysSuccesfullySaved(Gateway entityVersion)
        //{
        //    var options = DataBaseService.DataBaseProviderSetUp.SQLServerInMemoryOptions(nameof(AddGatewaysSuccesfullySaved));
        //    var context = new UnitOfWorkContainer(options);
        //    var gatewayRepository = new CaoFaturaRepository(context);
            
        //    await gatewayRepository.AddAsync(entityVersion);
        //    await gatewayRepository.UnitOfWork.CommitAsync();

        //    var g = await gatewayRepository.GetAsync(entityVersion.Id, new List<string>() { nameof(Gateway.Peripherals) });
        //    var gat = await gatewayRepository.GetAllAsync(new List<string>() { nameof(Gateway.Peripherals) },null);
            
        //    g.Should().NotBeNull();
        //    g.Peripherals.Should().NotBeNull();
        //    g.Peripherals.Count ().Should().Be(entityVersion.Peripherals.Count, "The same amount of related peripherals should be added");
        //}
    }
}
