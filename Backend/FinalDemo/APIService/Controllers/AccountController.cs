﻿using Domain.Authentication;
using Domain.Base;
using Domain.Models.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private static string Success = "Successfully";

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var result = await _accountRepository.SignInAsync(model);
            if (!(result.Message.IsNullOrEmpty())) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] string refreshToken)
        {
            var result = await _accountRepository.RefreshTokenAsync(refreshToken);
            if(!(result.Message.IsNullOrEmpty())) return Unauthorized(result.Message);
           return Ok(result);
        }


        [HttpGet("GetUserIdByEmail/{email}")]
        public async Task<IActionResult> GetUserIdByEmailAsync(string email)
        {
            var result = await _accountRepository.GetUserIdByEmailAsync(email);
            if (!(result.Length > 29)) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("GetAccountByUserId/{id}")]
        public async Task<IActionResult> GetAccountByUserIdAsync(string id)
        {
            var result = await _accountRepository.GetAccountByUserIdAsync(id);
            if (result==null) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("CreateMemberAccount")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _accountRepository.SignUpAsync(model);
            if (!result.Equals(Success)) return BadRequest(result);
            return Ok("Create Successfully. Next, confirm your email before login.");
        }

        [HttpPost("CreateVipAccount")]
        public async Task<IActionResult> SignUpVip(SignUpModel model)
        {
            var result = await _accountRepository.CreateVipAccount(model);
            if (!result.Equals(Success)) return BadRequest(result);
            return Ok("Create Successfully. Next, confirm your email before login.");
        }

        [HttpPost("CreateShopAccount")]
        public async Task<IActionResult> SignUpShop(SignUpModel model)
        {
            var result = await _accountRepository.CreateShopAccount(model);
            if (!result.Equals(Success)) return BadRequest(result);
            return Ok("Create Successfully. Next, confirm your email before login.");
        }

        [HttpPost("CreateAdminAccount")]
        public async Task<IActionResult> SignUpAdmin(SignUpModel model)
        {
            var result = await _accountRepository.CreateAdminAccount(model);
            if (!result.Equals(Success)) return BadRequest(result);
            return Ok("Create Successfully. Next, confirm your email before login.");
        }

        [HttpPost("ConfirmEmail/{email}/{code}")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            var result = await _accountRepository.ConfirmEmailAsync(email, code);
            if (!result.Equals(Success))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("RequestResetPassword/{email}")]
        public async Task<IActionResult> RequestResetPassword(string email)
        {
            var result = await _accountRepository.RequestPasswordResetAsync(email);
            if (!result.Equals(Success))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("ResetPassword/{email}/{token}")]
        public async Task<IActionResult> ResetPassword(string email, string token, NewPasswordModel newPass)
        {
            var result = await _accountRepository.ResetPasswordAsync(email, token, newPass);
            if (!result.Equals("Successfully"))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("ChangePassword{id}")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordModel model)
        {
            var result = await _accountRepository.ChangePasswordAsync(id, model);
            if (!result.Equals(Success)) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("UpdateAccountDetail{id}")]
        public async Task<IActionResult> UpdateAccountDetail(string id, AccountDetailModel model)
        {
            var result = await _accountRepository.UpdateAccountDetailAsync(id,model);
            if (!result.Equals(Success)) return BadRequest(result);
            return Ok(result);
        }       

        [HttpPut("ChangeToVipAccount{id}")]
        public async Task<IActionResult> ChangeRoleToVipAsync(string id)
        {
            var result = await _accountRepository.ChangeRoleToVipAsync(id);
            if (!result.Equals(Success)) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("LockoutEnable{id}")]
        public async Task<IActionResult> LockoutEnableAsync(string id)
        {
            var result = await _accountRepository.LockoutEnabledAsync(id);
           if(!result.Equals("Locked")) return BadRequest(result);
           return Ok(result);
        }

        [HttpPut("LockoutDisable{id}")]
        public async Task<IActionResult> LockoutDisableAsync(string id)
        {
            var result = await _accountRepository.LockoutDisabledAsync(id);
            if (!result.Equals("Unlocked")) return BadRequest(result);
            return Ok(result);
        }


        [HttpPut("DeleteAccount/{userId}")]
        public async Task<IActionResult> RemoveAccountById(string userId)
        {
            var result = await _accountRepository.RemoveAccountByIdAsync(userId);
            if (!result.Equals(Success)) return BadRequest(result);
            return Ok(result);
        }


    }
}
