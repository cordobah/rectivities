﻿using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);
        
        if (user == null) return Unauthorized();
        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (result)
        {
            return Ok(CreateUserDto(user));
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.UserName))
        {
            ModelState.AddModelError("username","Username is already taken");
            return ValidationProblem();
        }        
        
        if (await _userManager.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            ModelState.AddModelError("email","Email is already taken");
            return ValidationProblem();
        }

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.UserName
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return Ok(CreateUserDto(user));
        }

        return BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        return Ok(CreateUserDto(user));
    }
    
    private UserDto CreateUserDto(AppUser user)
    {
        return new UserDto
        {
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Username = user.UserName
        };
    }
}