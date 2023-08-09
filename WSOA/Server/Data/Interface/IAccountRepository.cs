﻿using WSOA.Shared.Entity;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Data.Interface
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Get an entity Account by Login and Password.
        /// </summary>
        public Account? GetByLoginAndPassword(SignInFormViewModel signInFormVM);
    }
}