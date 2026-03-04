using Application.DTO.RequestDTO.Auth;
using Application.DTO.RequestDTO.User;
using Application.DTO.ResponseDTO.Auth;
using Application.Interface;
using Helper;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuth
    {
        private readonly CommonRepositry _commonRepositry;
        private readonly IConfiguration _configuration;
        private readonly HrmdbContext _context;
        public AuthService(CommonRepositry commonRepositry, IConfiguration configuration, HrmdbContext context)
        {
            _commonRepositry = commonRepositry;
            _configuration = configuration;
            _context = context;
        }

        public async Task<CommonResponse> Login(LoginRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            LoginResponseDTO res = new LoginResponseDTO();
            try
            {
                var userDetail = await _commonRepositry.UserList().Select(x => new { x.Id, x.Email, x.Password, x.RoleId, x.Fullname,x.ProfilePic }).FirstOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password);
                if (userDetail != null)
                {
                    var roleDetail = await _commonRepositry.RoleList().FirstOrDefaultAsync(x => x.Id == userDetail.RoleId);
                    var tokenDetail = await _commonRepositry.TokenList().FirstOrDefaultAsync(x => x.UserId == userDetail.Id);
                    if (tokenDetail != null)
                    {
                        if (tokenDetail.TokenExpiryDate < DateTime.Now)
                        {
                            tokenDetail.RefreshTokenExpiryDate = (tokenDetail.RefreshTokenExpiryDate > DateTime.Now) ? tokenDetail.RefreshTokenExpiryDate : DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetSection("Jwt")["RefreshTokenExpiryMinutes"]));
                            tokenDetail.Token = await GenerateToken(userDetail.Id, Convert.ToInt32(userDetail.RoleId), roleDetail.RoleName);
                            tokenDetail.TokenExpiryDate = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetSection("Jwt")["DurationInMinutes"]));
                            tokenDetail.RefreshToken = await GenerateRefreshToken();
                            tokenDetail.UpdatedAt = DateTime.Now;

                            _context.Entry(tokenDetail).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                        res.Email = userDetail.Email;
                        res.RoleName = roleDetail.RoleName;
                        res.Token = tokenDetail.Token;
                        res.RefreshToken = tokenDetail.RefreshToken;
                        res.RoleId = Convert.ToInt32(userDetail.RoleId);
                        res.ProfilePic = userDetail.ProfilePic != null ? userDetail.ProfilePic : null;
                    }
                    else
                    {
                        var token = await GenerateToken(userDetail.Id, Convert.ToInt32(userDetail.RoleId), roleDetail.RoleName);
                        TokenMst tokenMst = new TokenMst();
                        tokenMst.UserId = userDetail.Id;
                        tokenMst.Token = token;
                        tokenMst.TokenExpiryDate = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetSection("Jwt")["DurationInMinutes"]));
                        tokenMst.RefreshToken = await GenerateRefreshToken();
                        tokenMst.RefreshTokenExpiryDate = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetSection("Jwt")["RefreshTokenExpiryMinutes"]));
                        tokenMst.CreatedAt = DateTime.Now;
                        tokenMst.UpdatedAt = DateTime.Now;

                        await _context.TokenMsts.AddAsync(tokenMst);
                        await _context.SaveChangesAsync();
                        res.Email = userDetail.Email;
                        res.RoleId = Convert.ToInt32(userDetail.RoleId);
                        res.RoleName = roleDetail.RoleName;
                        res.Token = tokenMst.Token;
                        res.RefreshToken = tokenMst.RefreshToken;
                    }

                    var rolePermissionList = await (from permission in _commonRepositry.PermissionList().Where(x => !x.IsDelete)
                                                    join rolePermission in _commonRepositry.RolePermissionList().Where(x => x.RoleId == userDetail.RoleId)
                                                    on permission.Id equals rolePermission.PermissionId into grp
                                                    from rolePermission in grp.DefaultIfEmpty()
                                                    select new LoginResponseDTO.PermissionDetail
                                                    {
                                                        PerrmissionName = permission.PermissionName,
                                                        HasPermission = rolePermission != null ? true : false,
                                                    }).ToListAsync();

                    res.PermissionDetailList = rolePermissionList;

                    response.Data = res;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Login successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "Data is invalid!";
                }
            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> RefreshToken(RefreshTokenRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            RefreshTokenResponseDTO responseDTO = new RefreshTokenResponseDTO();
            try
            {

                var tokenDetail = await _commonRepositry.TokenList().FirstOrDefaultAsync(x => x.Token.Trim().ToLower() == request.Token.Trim().ToLower() && x.RefreshToken.ToLower().Trim() == request.RefreshToken.Trim().ToLower());
                if (tokenDetail != null)
                {
                    if (tokenDetail.TokenExpiryDate < DateTime.Now)
                    {
                        if (tokenDetail.RefreshTokenExpiryDate > DateTime.Now)
                        {
                            var userDetail = await _commonRepositry.UserList().Select(x => new { x.Id, x.RoleId, x.Fullname }).FirstOrDefaultAsync(x => x.Id == tokenDetail.UserId);
                            if (userDetail != null)
                            {
                                var roleDetail = await _commonRepositry.RoleList().FirstOrDefaultAsync(x => x.Id == userDetail.RoleId);
                                tokenDetail.RefreshTokenExpiryDate = tokenDetail.RefreshTokenExpiryDate;
                                tokenDetail.Token = await GenerateToken(userDetail.Id, Convert.ToInt32(userDetail.RoleId), roleDetail.RoleName);
                                tokenDetail.TokenExpiryDate = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetSection("Jwt")["DurationInMinutes"]));
                                tokenDetail.RefreshToken = await GenerateRefreshToken();
                                tokenDetail.UpdatedAt = DateTime.Now;

                                _context.Entry(tokenDetail).State = EntityState.Modified;
                                await _context.SaveChangesAsync();

                                responseDTO.Token = tokenDetail.Token;
                                responseDTO.RefreshToken = tokenDetail.RefreshToken;
                                response.StatusCode = HttpStatusCode.OK;
                                response.Status = true;
                                response.Message = "RefreshToken generated successfully!";
                            }
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.BadRequest;
                            response.Status = false;
                            response.Message = "Your refreshtoken is  expired!";
                        }
                    }
                    else
                    {
                        responseDTO.Token = tokenDetail.Token;
                        responseDTO.RefreshToken = tokenDetail.RefreshToken;


                        response.StatusCode = HttpStatusCode.OK;
                        response.Status = true;
                        response.Message = "Your token is not expired!";
                    }

                    response.Data = responseDTO;
                    response.Message = "Refreshtoken get Successfully!";
                    response.Status = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch { throw; }
            return response;
        }

        private async Task<string> GenerateToken(int userId, int Role, string roleName)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var claims = new[]
            {
                new Claim("UserId", userId.ToString()),
                new Claim("RoleId",Role.ToString()),
                new Claim("RoleName",roleName.ToString()),
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<CommonResponse> RegisterUser(AddEditUserPersonalInfoRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {

                    UserMst userMst = new UserMst();
                    userMst.Email = request.Email;
                    userMst.Password = request.Password;
                    userMst.ConfirmPassword = request.ConfirmPassword;
                    userMst.AadharNumber = request.AadharNumber;
                    userMst.Gender = request.Gender;
                    userMst.UserId = request.UserId;
                    userMst.Contact = request.ContactNo;
                    userMst.IsActive = true;
                    userMst.CreatedAt = DateTime.Now;
                    userMst.CreatedBy = 1;
                    userMst.IsDelete = false;
                    userMst.UpdatedAt = DateTime.Now;
                    userMst.UpdatedBy = 1;
                    userMst.Pincode = request.PinCode;
                    userMst.Fullname = request.FullName;
                    userMst.Address = request.Address;

                    if (request.ProfilePic != null && request.ProfilePic.Length > 0)
                    {
                        string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        string folderPath = Path.Combine(wwwRootPath, "UserAttachment");

                        string fileName = userMst.UserId + Path.GetExtension(request.ProfilePic.FileName);

                        // Full path for save
                        string filePath = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.ProfilePic.CopyToAsync(stream);
                        }
                        userMst.ProfilePic = $"/UserAttachment/{fileName}";
                    }

                    await _context.UserMsts.AddAsync(userMst);
                    await _context.SaveChangesAsync();

                    response.Data = userMst.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "User register Successfully!";
            }
            catch { throw; }
            return response;
        }

    }
}
