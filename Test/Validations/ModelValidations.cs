using Application.DataAnnotations;
using Application.Dtos;
using Application.Exceptions;
using FluentAssertions;
using Infraestructure.Application.Validator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Validations
{
    [TestFixture]
    [Parallelizable]
    public class ModelValidations
    {
        #region Data Sets
        private static Guid gatewayId = Guid.NewGuid();

        //private static readonly object[] GatewaysDataSet =
        //{
        //    // Validation Fails
        //    new object[]{ null,false},
        //    new object[]{ new GatewayDto{Id = Guid.NewGuid(),Name=null,Ipv4Address=null },false},
        //    new object[]{ new GatewayDto{Id = Guid.NewGuid(),Name=string.Empty,Ipv4Address=string.Empty },false},
        //    new object[]{ new GatewayDto{Id = Guid.NewGuid(),Name=string.Empty,Ipv4Address="4999.168.10.1" },false},
        //    new object[]{ new GatewayDto{Id = Guid.NewGuid(),Name="Name 1",Ipv4Address=string.Empty },false},
        //    new object[]{ new GatewayDto{Id = Guid.NewGuid(),Name="Name 1",Ipv4Address="4999.168.10.1" },false},
        //    new object[]{ new GatewayDto{Id = Guid.NewGuid(),Name="Name 1",Ipv4Address="192.4999.10.1" },false},
        //    new object[]{ new GatewayDto{Id = gatewayId,Name="Name 1",Ipv4Address="192.168.10.1",
        //        Peripherals=new List<PeripheralDto>() {
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=gatewayId},
        //    } }, false},

        //    //Validation Success
        //    new object[]{ new GatewayDto{Id = Guid.NewGuid(),Name="Name 1",Ipv4Address="192.168.10.1" },true},
        //    new object[]{ new GatewayDto{Id = new Guid("68cc3e5e-01be-459b-84d1-8d03bf1f6f58"),Name="Name 1",Ipv4Address="192.168.10.1",
        //        Peripherals=new List<PeripheralDto>() {
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=new Guid("68cc3e5e-01be-459b-84d1-8d03bf1f6f58")},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=new Guid("68cc3e5e-01be-459b-84d1-8d03bf1f6f58")},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=new Guid("68cc3e5e-01be-459b-84d1-8d03bf1f6f58")},
        //        new PeripheralDto{ Id = Guid.NewGuid(), Vendor="Pa & Co",CreatedDate=DateTime.Today,Status=true, GatewayId=new Guid("68cc3e5e-01be-459b-84d1-8d03bf1f6f58")},
        //    } }, true},
        //};

        #endregion


        //[Test]
        //[TestCaseSource(nameof(GatewaysDataSet))]
        //public void GatewaysValidations(GatewayDto item, bool result)
        //{
        //    var validator = new DataAnnotationsEntityValidator();
        //    var validationResult = validator.IsValid(item);
        //    validationResult.Should().Be(result);
        //}



        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("399.168.10.1", false)]
        [TestCase("192.399.10.1", false)]
        [TestCase("192.168.399.1", false)]
        [TestCase("192.168.10.399", false)]
        [TestCase("192.168.10.1", true)]
        public void ValidatingIpAddressAttribute(string ipAddress, bool expectedResult)
        {
            var ipv4AddressAttribute = new Ipv4Address();

            var validationResult = ipv4AddressAttribute.IsValid(ipAddress);
            validationResult.Should().Be(expectedResult);
        }
    }
}
