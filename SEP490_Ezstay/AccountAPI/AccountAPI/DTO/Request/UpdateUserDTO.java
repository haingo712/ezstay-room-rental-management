package AccountAPI.DTO.Request;

import AccountAPI.Enums.GenderEnum;

public class UpdateUserDTO {
	public final GenderEnum _gender;
	public final String _bio;
	public final DateTime _dateOfBirth;
	public final IFormFile _avatar;
	public final String _fullName;
	public final String _phone;
	public final String _detailAddress;
	public final String _provinceId;
	public final String _communeId;
	public GenderEnum _unnamed_GenderEnum_;

	public GenderEnum getGender() {
		return this._gender;
	}

	public String getBio() {
		return this._bio;
	}

	public DateTime getDateOfBirth() {
		return this._dateOfBirth;
	}

	public IFormFile getAvatar() {
		return this._avatar;
	}

	public String getFullName() {
		return this._fullName;
	}

	public String getPhone() {
		return this._phone;
	}

	public String getDetailAddress() {
		return this._detailAddress;
	}

	public String getProvinceId() {
		return this._provinceId;
	}

	public String getCommuneId() {
		return this._communeId;
	}
}