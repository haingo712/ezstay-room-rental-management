package AccountAPI.Controllers;

import AccountAPI.Service.Interfaces.IUserService;

public class UserController {
	private IUserService _userService;
	private IUserClaimHelper _userClaimHelper;
	public IUserService _unnamed_IUserService_;

	public UserController(IUserService aUserService, IUserClaimHelper aUserClaimHelper) {
		throw new UnsupportedOperationException();
	}

	public async_Task<IActionResult> CreateProfile(UserDTO aUserDto) {
		throw new UnsupportedOperationException();
	}

	public async_Task<IActionResult> GetProfile() {
		throw new UnsupportedOperationException();
	}

	public async_Task<IActionResult> UpdatePhone(UpdatePhoneRequestDto aDto) {
		throw new UnsupportedOperationException();
	}

	public async_Task<IActionResult> UpdateProfile(UpdateUserDTO aDto) {
		throw new UnsupportedOperationException();
	}

	public async_Task<IActionResult> UpdateEmail(UpdateEmailRequestDto aDto) {
		throw new UnsupportedOperationException();
	}
}