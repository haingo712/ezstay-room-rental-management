package AccountAPI.DTO.Reponse;

import AccountAPI.Enums.GenderEnum;

public class UserResponseDTO {
	public final Guid _id;
	public final String _adrress;
	public final GenderEnum _gender;
	public final String _avatar;
	public final String _bio;
	public final DateTime _dateOfBirth;
	public final Guid _userId;
	public final String _fullName;
	public final String _phone;
	public final String _detailAddress;
	public final String _provinceCode;
	public final String _provinceName;
	public final String _communeCode;
	public final String _communeName;
	public GenderEnum _unnamed_GenderEnum_;

	public Guid getId() {
		return this._id;
	}

	public String getAdrress() {
		return this._adrress;
	}

	public GenderEnum getGender() {
		return this._gender;
	}

	public String getAvatar() {
		return this._avatar;
	}

	public String getBio() {
		return this._bio;
	}

	public DateTime getDateOfBirth() {
		return this._dateOfBirth;
	}

	public Guid getUserId() {
		return this._userId;
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

	public String getProvinceCode() {
		return this._provinceCode;
	}

	public String getProvinceName() {
		return this._provinceName;
	}

	public String getCommuneCode() {
		return this._communeCode;
	}

	public String getCommuneName() {
		return this._communeName;
	}
}