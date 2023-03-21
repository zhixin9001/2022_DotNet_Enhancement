namespace IdentityService.Infrastructure;

public class IdRepository : IIdRepository
{
    private readonly IdUserManager _userManager;
    private readonly RoleManager<Role> _roleManager;

    public IdRepository(IdUserManager userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public Task<User?> FindByIdAsync(Guid userId)
    {
        return _userManager.FindByIdAsync(userId.ToString());
    }

    public Task<User?> FindByNameAsync(string userName)
    {
        return _userManager.FindByNameAsync(userName);
    }

    public Task<User?> FindByPhoneNumberAsync(string phoneNum)
    {
        return _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNum);
    }

    public Task<IdentityResult> CreateAsync(User user, string password)
    {
        return _userManager.CreateAsync(user, password);
    }

    public Task<IdentityResult> AccessFailedAsync(User user)
    {
        return _userManager.AccessFailedAsync(user);
    }

    public Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber)
    {
        return _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
    }

    public async Task<SignInResult> ChangePhoneNumAsync(Guid userId, string phoneNum, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new ArgumentException($"user:{userId} not exist");
        }

        var changeResult = await _userManager.ChangePhoneNumberAsync(user, phoneNum, token);
        if (!changeResult.Succeeded)
        {
            await _userManager.AccessFailedAsync(user);
            var errMsg = changeResult.Errors.SumErrors();
            return SignInResult.Failed;
        }
        else
        {
            await ConfirmPhoneNumberAsync(user.Id);
            return SignInResult.Success;
        }
    }

    public async Task<IdentityResult> ChangePasswordAsync(Guid userId, string password)
    {
        if (password.Length < 6)
        {
            IdentityError error = new()
            {
                Code = "Password invalid",
                Description = "Cant be less than 6",
            };
            return IdentityResult.Failed(error);
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetPwdResult = await _userManager.ResetPasswordAsync(user, token, password);
        return resetPwdResult;
    }

    public Task<IList<string>> GetRolesAsync(User user)
    {
        return _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var result = await _roleManager.CreateAsync(new() {Name = role});
            if (!result.Succeeded)
            {
                return result;
            }
        }

        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure)
    {
        if (await _userManager.IsLockedOutAsync(user))
        {
            return SignInResult.LockedOut;
        }

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (result)
        {
            return SignInResult.Success;
        }
        else
        {
            if (lockoutOnFailure)
            {
                var r = await AccessFailedAsync(user);
                if (!r.Succeeded)
                {
                    throw new ApplicationException("AccessFailed failed");
                }
            }

            return SignInResult.Failed;
        }
    }

    public async Task ConfirmPhoneNumberAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            throw new ArgumentException($"Cannot find user {id}");
        }

        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);
    }

    public async Task UpdatePhoneNumberAsync(Guid id, string phoneNum)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            throw new ArgumentException($"Cannot find user {id}");
        }

        user.PhoneNumber = phoneNum;
        await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> RemoveUserAsync(Guid id)
    {
        var user = await FindByIdAsync(id);
        var userLoginStore = _userManager.UserLoginStore;
        var noneCT = default(CancellationToken);
        //一定要删除aspnetuserlogins表中的数据，否则再次用这个外部登录登录的话
        //就会报错：The instance of entity type 'IdentityUserLogin<Guid>' cannot be tracked because another instance with the same key value for {'LoginProvider', 'ProviderKey'} is already being tracked.
        //而且要先删除aspnetuserlogins数据，再软删除User
        var logins = await userLoginStore.GetLoginsAsync(user, noneCT);
        foreach (var login in logins)
        {
            await userLoginStore.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey, noneCT);
        }

        user.SoftDelete();
        var result = await _userManager.UpdateAsync(user);
        return result;
    }

    private static IdentityResult ErrorResult(string msg)
    {
        IdentityError idError = new IdentityError {Description = msg};
        return IdentityResult.Failed(idError);
    }

    private string GeneratePassword()
    {
        var options = _userManager.Options.Password;
        int length = options.RequiredLength;
        bool nonAlphanumeric = options.RequireNonAlphanumeric;
        bool digit = options.RequireDigit;
        bool lowercase = options.RequireLowercase;
        bool uppercase = options.RequireUppercase;
        StringBuilder password = new StringBuilder();
        Random random = new Random();
        while (password.Length < length)
        {
            char c = (char) random.Next(32, 126);
            password.Append(c);
            if (char.IsDigit(c))
                digit = false;
            else if (char.IsLower(c))
                lowercase = false;
            else if (char.IsUpper(c))
                uppercase = false;
            else if (!char.IsLetterOrDigit(c))
                nonAlphanumeric = false;
        }

        if (nonAlphanumeric)
            password.Append((char) random.Next(33, 48));
        if (digit)
            password.Append((char) random.Next(48, 58));
        if (lowercase)
            password.Append((char) random.Next(97, 123));
        if (uppercase)
            password.Append((char) random.Next(65, 91));
        return password.ToString();
    }

    public async Task<(IdentityResult, User?, string? password)> AddAdminUserAsync(string userName, string phoneNum)
    {
        if (await FindByNameAsync(userName) != null)
        {
            return (ErrorResult($"User {userName} already exist"), null, null);
        }

        if (await FindByPhoneNumberAsync(phoneNum) != null)
        {
            return (ErrorResult($"PhoneNum {phoneNum} already exist"), null, null);
        }

        User user = new(userName)
        {
            PhoneNumber = phoneNum,
            PhoneNumberConfirmed = true
        };
        var password = GeneratePassword();
        var result = await CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return (result, null, null);
        }

        return (IdentityResult.Success, user, password);
    }

    public async Task<(IdentityResult, User?, string? password)> ResetPasswordAsync(Guid id)
    {
        var user = await FindByIdAsync(id);
        if (user == null)
        {
            return (ErrorResult("User not exist"), null, null);
        }

        var password = GeneratePassword();
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded)
        {
            return (result, null, null);
        }

        return (IdentityResult.Success, user, password);
    }
}