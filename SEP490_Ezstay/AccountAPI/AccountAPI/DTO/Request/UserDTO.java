package AccountAPI.DTO.Request;

import AccountAPI.Enums.GenderEnum;

public class UserDTO {
	public final GenderEnum _gender;
	public final String _avatar;
	public final String _bio;
	public final String _provinceCode;
	public final String _communeCode;
	public final String _detailAddress;
	public GenderEnum _unnamed_GenderEnum_;

	public GenderEnum getGender() {
		return this._gender;
	}

	public String getAvatar() {
		return this._avatar;
	}

	public String getBio() {
		return this._bio;
	}

	public String getProvinceCode() {
		return this._provinceCode;
	}

	public String getCommuneCode() {
		return this._communeCode;
	}

	public String getDetailAddress() {
		return this._detailAddress;
	}
}