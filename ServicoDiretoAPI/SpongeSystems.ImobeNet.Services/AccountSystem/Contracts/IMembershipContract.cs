using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web.Mvc;
using SpongeSolutions.Core;
using SpongeSolutions.ServicoDireto.Model.AccountSystem;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.Core.Model;

namespace SpongeSolutions.ServicoDireto.Services.AccountSystem.Contracts
{
    public interface IMembershipContract
    {
        #region [Properties]
        int MinPasswordLength { get; }
        #endregion

        /// <summary>
        /// Efetua a validação de um determinado usuario
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ValidateUser(string userName, string password);

        /// <summary>
        /// Efetua a criacao de um usuário
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        MembershipCreateStatus CreateUser(RegisterModel register);

        /// <summary>
        /// Efetua a alteracao dos dados do usuario
        /// </summary>
        /// <param name="register"></param>
        void ChangeUser(RegisterModel register);

        /// <summary>
        /// Efetua a alteração da senha de um determinado usuário
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        bool ChangePassword(string userName, string oldPassword, string newPassword);

        bool ChangePassword(string userName, string newPassword);

        /// <summary>
        /// Listar todos os usuários que estiverem em uma determinada Role
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        IEnumerable<RegisterModel> ListUsers(int startRowIndex = -1, int maximumRows = -1, string roleName = null);

        /// <summary>
        /// Buscar um usuário pelo seu nome
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IEnumerable<RegisterModel> GetUserByName(string userName, int startRowIndex = -1, int maximumRows = -1);

        /// <summary>
        /// Buscar Usuário por Email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        IEnumerable<RegisterModel> GetUserByEmail(string email, int startRowIndex = -1, int maximumRows = -1);

        /// <summary>
        /// Verifica a existência de um determinado usuário.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool VerifyUserExistence(string userName);

        /// <summary>
        /// Listar todos os papéis disponíveis
        /// </summary>
        /// <returns></returns>
        string[] ListRoles();

        /// <summary>
        /// Vincular usuário a papeis
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="userName"></param>
        void AddUserToRoles(string[] roles, string userName);

        /// <summary>
        /// Listar a roles que estão vinculadas a um determinado usuario
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IEnumerable<System.Web.Mvc.SelectListItem> ListRelatedRoles(string userName);

        /// <summary>
        /// Listar os papeis disponiveis para o usuário em questão
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IEnumerable<System.Web.Mvc.SelectListItem> ListAvailableRoles(string userName);

        /// <summary>
        /// Listar todos os papeis e trazer selecionados os que estão vinculado ao usuario em questão
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IEnumerable<CustomSelectListItem> ListRoles(string userName);

        void DeleteUser(string[] userName);

        IEnumerable<SelectListItem> ListAllUsers();
    }
}
