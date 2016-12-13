﻿namespace App.Test.Security.Role
{
    using App.Common;
    using App.Common.DI;
    using App.Common.Validation;
    using App.Service.Security.Role;
    using Common.UnitTest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class CreateRoleTest : BaseUnitTest
    {
        [TestMethod]
        public void Security_Role_CreateRole_ShouldBeSuccess_WithValidRequest()
        {
            string name = "Name of Role" + Guid.NewGuid().ToString("N");
            string description = "Desc of Role";
            CreateRoleResponse role = this.CreateRoleItem(name, description);
            IRoleService service = IoC.Container.Resolve<IRoleService>();
            Assert.IsNotNull(service.GetRole(role.Id));
        }

        [TestMethod]
        public void Security_Role_CreateRole_ShouldGetException_WithEmptyName()
        {
            try
            {
                string name = string.Empty;
                string description = "Desc of Role";
                string key = "Key of Role" + Guid.NewGuid().ToString("N");
                this.CreateRoleItem(name, description);
                Assert.IsTrue(false);
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.HasExceptionKey("security.addOrUpdateRole.validation.nameIsRequired"));
            }
        }

        [TestMethod]
        public void Security_Role_CreateRole_ShouldGetException_WithDuplicatedName()
        {
            try
            {
                string name = "Name of Role" + Guid.NewGuid().ToString("N");
                string description = "Desc of Role";
                this.CreateRoleItem(name, description);
                this.CreateRoleItem(name, description);
                Assert.IsTrue(false);
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.HasExceptionKey("security.addOrUpdateRole.validation.nameAlreadyExisted"));
            }
        }

        [TestMethod]
        public void Security_Role_CreateRole_ShouldGetException_WithDuplicatedKey()
        {
            try
            {
                string newGuid = Guid.NewGuid().ToString("N");
                string name = "Name of Role" + newGuid;
                string name1 = "Name_of_Role" + newGuid;
                string description = "Desc of Role";
                this.CreateRoleItem(name, description);
                this.CreateRoleItem(name1, description);
                Assert.IsTrue(false);
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.HasExceptionKey("security.addOrUpdateRole.validation.keyAlreadyExisted"));
            }
        }

        private CreateRoleResponse CreateRoleItem(string name, string desc)
        {
            CreateRoleRequest request = new CreateRoleRequest(name, desc);
            IRoleService service = IoC.Container.Resolve<IRoleService>();
            return service.Create(request);
        }
    }
}