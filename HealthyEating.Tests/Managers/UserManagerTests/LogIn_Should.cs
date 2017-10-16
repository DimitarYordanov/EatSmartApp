﻿using HealthyEating.Client.Core.Contracts;
using HealthyEating.Client.Data;
using HealthyEating.Client.Managers;
using HealthyEating.Client.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace HealthyEating.Tests.Managers.UserManagerTests
{
    [TestClass]
    public class LogIn_Should
    {
        [TestMethod]
        public void ThrowArgumentNullException_WhenUsernameIsNull()
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object,databaseMock.Object,modelFactoryMock.Object);
            var password = "somelegitpassword";
            string username = null;
            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(()=>userManager.LogIn(username, password));
        }

        [TestMethod]
        [DataRow(" ")]
        [DataRow("")]
        public void ThrowArgumentException_WhenUsernameIsEmptyOrWhiteSpace(string username)
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object, databaseMock.Object, modelFactoryMock.Object);
            var password = "somelegitpassword";
            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => userManager.LogIn(username, password));
        }

        [TestMethod]
        public void ThrowArgumentNullException_WhenPasswordIsNull()
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object, databaseMock.Object, modelFactoryMock.Object);
            string password = null ;
            string username = "somelegitusername";
            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => userManager.LogIn(username, password));
        }

        [TestMethod]
        [DataRow(" ")]
        [DataRow("")]
        public void ThrowArgumentException_WhenPasswordIsEmptyOrWhiteSpace(string password)
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object, databaseMock.Object, modelFactoryMock.Object);
            var username = "somelegitusername";
            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => userManager.LogIn(username, password));
        }

        [TestMethod]
        public void ThrowArgumentExceptionWithCustomMessage_WhenUsernameIsIncorrect()
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object, databaseMock.Object, modelFactoryMock.Object);
            var username = "usernameWhichIsNotContainedInTheDB";
            var password = "passwordWhichIsContainedInTheDB";
            var usersListMock = new List<User>();

            databaseMock.SetupGet(x => x.Users).Returns(usersListMock);

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(()=>userManager.LogIn(username, password), "Wrong username or password");         
        }

        [TestMethod]
        public void ThrowArgumentExceptionWithCustomMessage_WhenPasswordIsIncorrect()
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object, databaseMock.Object, modelFactoryMock.Object);
            var username = "usernameWhichIsContainedInTheDB";
            var password = "passwordWhichIsNotContainedInTheDB";
            var usersListMock = new List<User>();
            usersListMock.Add(new User() { Username = username, Password = "passwordDifferentFromOurs" });

            databaseMock.SetupGet(x => x.Users).Returns(usersListMock);
            passwordHasherMock.Setup(x => x.Verify(password, "passwordDifferentFromOurs")).Returns(false);

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => userManager.LogIn(username, password), "Wrong username or password");
        }

        [TestMethod]
        public void ThrowArgumentExceptionWithCustomMessage_WhenUserIsDeleted()
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object, databaseMock.Object, modelFactoryMock.Object);
            var username = "usernameWhichIsContainedInTheDB";
            var password = "passwordWhichIsContainedInTheDB";
            var usersListMock = new List<User>();
            usersListMock.Add(new User() { Username = username, Password = password,IsDeleted=true });

            databaseMock.SetupGet(x => x.Users).Returns(usersListMock);
            passwordHasherMock.Setup(x => x.Verify(password, password)).Returns(true);

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => userManager.LogIn(username, password), "Wrong username or password");
        }

        [TestMethod]
        public void MakeLoggedUserTheCurrentUser_WhenParametersAreCorrect()
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object, databaseMock.Object, modelFactoryMock.Object);
            var username = "usernameWhichIsContainedInTheDB";
            var password = "passwordWhichIsContainedInTheDB";
            var usersListMock = new List<User>();
            usersListMock.Add(new User() { Username = username, Password = password});

            databaseMock.SetupGet(x => x.Users).Returns(usersListMock);
            passwordHasherMock.Setup(x => x.Verify(password, password)).Returns(true);

            //Act 
            userManager.LogIn(username, password);

            //Assert
            Assert.AreSame(usersListMock[0], userManager.LoggedUser);
        }

        [TestMethod]
        public void ReturnSuccessMessage_WhenParametersAreCorrect()
        {
            //Arrange
            var databaseMock = new Mock<IDatabase>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var modelFactoryMock = new Mock<IModelFactory>();
            var userManager = new UserManager(passwordHasherMock.Object, databaseMock.Object, modelFactoryMock.Object);
            var username = "usernameWhichIsContainedInTheDB";
            var password = "passwordWhichIsContainedInTheDB";
            var usersListMock = new List<User>();
            var expected=$"Hi, {username}!";
            usersListMock.Add(new User() { Username = username, Password = password });

            databaseMock.SetupGet(x => x.Users).Returns(usersListMock);
            passwordHasherMock.Setup(x => x.Verify(password, password)).Returns(true);

            //Act 
            string result=userManager.LogIn(username, password);

            //Assert
            Assert.AreEqual(expected,result);
        }
    }
}