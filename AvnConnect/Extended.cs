using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AvnConnect
{
    namespace Data
    {
        public partial class MyStaff : INotifyPropertyChanged
        {
            /// <summary>
            /// Triển khai INotifyPropertyChanged
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Thông báo về việc thay đổi giá trị của 1 thuộc tính
            /// Cần được gọi trong hàm set của mỗi thuộc tính
            /// </summary>
            /// <param name="propertyName"></param>
            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    Console.WriteLine("Property: {0} has changed!", propertyName);
                }
            }

            internal Data.Staff ConvertToEfStaff()
            {
                Data.Staff NewStaff = new Data.Staff();
                MiscUtil.Reflection.PropertyCopy.Copy(this, NewStaff);
                return NewStaff;
            }


            internal void FromEframeStaff(Data.Staff NewStaff)
            {
                MiscUtil.Reflection.PropertyCopy.Copy(NewStaff, this);
            }


            #region FIELDS
            private int _Id;
            private string _Surname;
            private string _Firstname;
            private Nullable<DateTime> _Birthday;
            private string _PlaceOfBirth;
            private Nullable<DateTime> _FirstDayAtWork;
            private Nullable<DateTime> _LastDayAtWork;
            private string _JobTitle;
            private string _Department;
            private string _Nationality;
            private Nullable<bool> _Gender;
            private string _HomePhone;
            private string _MobilePhone;
            private string _MaritalStatus;
            private string _IDNumber;
            private Nullable<System.DateTime> _DateOf_ID_Issue;
            private string _PlaceOf_ID_Issue;
            private string _PassportNumber;
            private Nullable<System.DateTime> _DateOfPassportIssue;
            private string _PlaceOfPassportIssue;
            private string _EmailAddress;
            private string _Email2;
            private string _PermanentResidence;
            private string _CurrentAddress;
            private string _SocialInsuranceNumber;
            private string _PIT_Code;
            private string _PIT_Deduction;
            private string _VCBAccount;
            private string _OtherBankAccount;
            private string _OtherBankAccountSubsidiary;
            private string _EmergencyContactName;
            private string _EmergencyContactPhone;
            private string _EmergencyContactRelationship;
            private string _Key;
            private string _Avatar;
            private System.DateTime _AddedOn;
            private string _AddedBy;
            private string _ModifiedOn;
            private string _ModifiedBy;
            private string _Password;
            #endregion

            #region PROPERTIES
            /// <summary>
            /// ID của nhân viên
            /// </summary>
            public int Id
            {
                get
                {
                    return this._Id;
                }
                set
                {
                    if (value != this._Id)
                    {
                        this._Id = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Họ của nhân viên
            /// </summary>
            public string Surname
            {
                get
                {
                    return this._Surname;
                }
                set
                {
                    if (value != this._Surname)
                    {
                        this._Surname = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Tên của nhân viên
            /// </summary>
            public string Firstname
            {
                get
                {
                    return this._Firstname;
                }
                set
                {
                    if (value != this._Firstname)
                    {
                        this._Firstname = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Ngày sinh
            /// </summary>
            public Nullable<System.DateTime> Birthday
            {
                get
                {
                    return this._Birthday;
                }
                set
                {
                    if (value != this._Birthday)
                    {
                        this._Birthday = value;
                        NotifyPropertyChanged();
                    }
                }

            }

            /// <summary>
            /// Nơi sinh
            /// </summary>
            public string PlaceOfBirth
            {
                get
                {
                    return this._PlaceOfBirth;
                }
                set
                {
                    if (value != this._PlaceOfBirth)
                    {
                        this._PlaceOfBirth = value;
                        NotifyPropertyChanged();
                    }
                }

            }


            /// <summary>
            /// Ngày làm việc đầu tiên
            /// </summary>
            public Nullable<System.DateTime> FirstDayAtWork
            {
                get
                {
                    return this._FirstDayAtWork;
                }
                set
                {
                    if (value != this._FirstDayAtWork)
                    {
                        this._FirstDayAtWork = value;
                        this.NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Ngày làm việc cuối cùng
            /// </summary>
            public Nullable<System.DateTime> LastDayAtWork
            {
                get
                {
                    return this._LastDayAtWork;
                }
                set
                {
                    if (value != this._LastDayAtWork)
                    {
                        this._LastDayAtWork = value;
                        this.NotifyPropertyChanged();
                    }
                }
            }


            /// <summary>
            /// Chức vụ
            /// </summary>
            public string JobTitle
            {
                get { return this._JobTitle; }
                set
                {
                    if (value != this._JobTitle)
                    {
                        this._JobTitle = value;
                        NotifyPropertyChanged();
                    }
                }

            }

            /// <summary>
            /// Phòng ban
            /// </summary>
            public string Department
            {
                get { return this._Department; }
                set
                {
                    if (value != this._Department)
                    {
                        this._Department = value;
                        NotifyPropertyChanged();
                    }
                }

            }

            /// <summary>
            /// Quốc tịch
            /// </summary>
            public string Nationality
            {
                get { return this._Nationality; }
                set
                {
                    if (value != this._Nationality)
                    {
                        this._Nationality = value;
                        NotifyPropertyChanged();
                    }
                }

            }


            /// <summary>
            /// Giới tính
            /// true = male
            /// false = female
            /// null = unset
            /// </summary>
            public Nullable<bool> Gender
            {
                get { return _Gender; }
                set
                {
                    if (value != _Gender)
                    {
                        _Gender = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            

            /// <summary>
            /// Số điện thoại nhà riêng
            /// </summary>
            public string HomePhone
            {
                get { return this._HomePhone; }
                set
                {
                    if (value != this._HomePhone)
                    {
                        this._HomePhone = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Số điện thoại di động
            /// </summary>
            public string MobilePhone
            {
                get { return this._MobilePhone; }
                set
                {
                    if (value != this._MobilePhone)
                    {
                        this._MobilePhone = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Tình trạng hôn nhân
            /// </summary>
            public string MaritalStatus
            {
                get { return this._MaritalStatus; }
                set
                {
                    if (value != this._MaritalStatus)
                    {
                        this._MaritalStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Số chứng minh nhân dân
            /// </summary>
            public string IDNumber
            {
                get { return this._IDNumber; }
                set
                {
                    if (value != this._IDNumber)
                    {
                        this._IDNumber = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Ngày cấp chứng minh nhân dân
            /// </summary>
            public Nullable<System.DateTime> DateOf_ID_Issue
            {
                get { return this._DateOf_ID_Issue; }
                set
                {
                    if (value != this._DateOf_ID_Issue)
                    {
                        this._DateOf_ID_Issue = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Nơi cấp CMND
            /// </summary>
            public string PlaceOf_ID_Issue
            {
                get { return this._PlaceOf_ID_Issue; }
                set
                {
                    if (value != this._PlaceOf_ID_Issue)
                    {
                        this._PlaceOf_ID_Issue = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Số hộ chiếu
            /// </summary>
            public string PassportNumber
            {
                get { return this._PassportNumber; }
                set
                {
                    if (value != this._PassportNumber)
                    {
                        this._PassportNumber = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Ngày cấp hộ chiếu
            /// </summary>
            public Nullable<System.DateTime> DateOfPassportIssue
            {
                get { return this._DateOfPassportIssue; }
                set
                {
                    if (value != this._DateOfPassportIssue)
                    {
                        this._DateOfPassportIssue = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Nơi cấp hộ chiếu
            /// </summary>
            public string PlaceOfPassportIssue
            {
                get { return this._PlaceOfPassportIssue; }
                set
                {
                    if (value != this._PlaceOfPassportIssue)
                    {
                        this._PlaceOfPassportIssue = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Địa chỉ Email
            /// </summary>
            public string EmailAddress
            {
                get { return this._EmailAddress; }
                set
                {
                    if (value != this._EmailAddress)
                    {
                        this._EmailAddress = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Địa chỉ Email
            /// </summary>
            public string Email2
            {
                get { return this._Email2; }
                set
                {
                    if (value != this._Email2)
                    {
                        this._Email2 = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Địa chỉ thường trú
            /// </summary>
            public string PermanentResidence
            {
                get { return this._PermanentResidence; }
                set
                {
                    if (value != this._PermanentResidence)
                    {
                        this._PermanentResidence = value;
                        NotifyPropertyChanged();
                    }
                }
            }


            /// <summary>
            /// Địa chỉ hiện tại
            /// </summary>
            public string CurrentAddress
            {
                get { return this._CurrentAddress; }
                set
                {
                    if (value != this._CurrentAddress)
                    {
                        this._CurrentAddress = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Số sổ BHXH
            /// </summary>
            public string SocialInsuranceNumber
            {
                get { return this._SocialInsuranceNumber; }
                set
                {
                    if (value != this._SocialInsuranceNumber)
                    {
                        this._SocialInsuranceNumber = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Mã số thuế TNCN
            /// </summary>
            public string PIT_Code
            {
                get { return this._PIT_Code; }
                set
                {
                    if (value != this._PIT_Code)
                    {
                        this._PIT_Code = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Tình trạng giảm trừ gia cảnh
            /// </summary>
            public string PIT_Deduction
            {
                get { return this._PIT_Deduction; }
                set
                {
                    if (value != this._PIT_Deduction)
                    {
                        this._PIT_Deduction = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Số tài khoản VCB
            /// </summary>
            public string VCBAccount
            {
                get { return this._VCBAccount; }
                set
                {
                    if (value != this._VCBAccount)
                    {
                        this._VCBAccount = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Số tài khoản ngân hàng khác
            /// </summary>
            public string OtherBankAccount
            {
                get { return this._OtherBankAccount; }
                set
                {
                    if (value != this._OtherBankAccount)
                    {
                        this._OtherBankAccount = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Chi nhánh ngân hàng mở Tài khoản
            /// </summary>
            public string OtherBankAccountSubsidiary
            {
                get { return this._OtherBankAccountSubsidiary; }
                set
                {
                    if (value != this._OtherBankAccountSubsidiary)
                    {
                        this._OtherBankAccountSubsidiary = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Người liên lạc khẩn cấp
            /// </summary>
            public string EmergencyContactName
            {
                get { return this._EmergencyContactName; }
                set
                {
                    if (value != this._EmergencyContactName)
                    {
                        this._EmergencyContactName = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Số điện thoại liên lạc khẩn cấp
            /// </summary>
            public string EmergencyContactPhone
            {
                get { return this._EmergencyContactPhone; }
                set
                {
                    if (value != this._EmergencyContactPhone)
                    {
                        this._EmergencyContactPhone = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Mối quan hệ với người liên lạc khẩn cấp
            /// </summary>
            public string EmergencyContactRelationship
            {
                get { return this._EmergencyContactRelationship; }
                set
                {
                    if (value != this._EmergencyContactRelationship)
                    {
                        this._EmergencyContactRelationship = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Mã nhân viên
            /// </summary>
            public string Key
            {
                get { return this._Key; }
                set
                {
                    if (value != this._Key)
                    {
                        this._Key = value;
                        NotifyPropertyChanged();
                    }
                }
            }


            /// <summary>
            /// Hình ảnh đại diện (base64)
            /// </summary>
            public string Avatar
            {
                get { return this._Avatar; }
                set
                {
                    if (value != this._Avatar)
                    {
                        this._Avatar = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Được thêm vào hệ thống lúc
            /// </summary>
            public System.DateTime AddedOn
            {
                get { return this._AddedOn; }
                set
                {
                    if (value != this._AddedOn)
                    {
                        this._AddedOn = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Được thêm vào bởi ai
            /// </summary>
            public string AddedBy
            {
                get { return this._AddedBy; }
                set
                {
                    if (value != this._AddedBy)
                    {
                        this._AddedBy = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Được sử đổi vào lúc
            /// </summary>
            public string ModifiedOn
            {
                get { return this._ModifiedOn; }
                set
                {
                    if (value != this._ModifiedOn)
                    {
                        this._ModifiedOn = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// Được sửa đổi bởi ai
            /// </summary>
            public string ModifiedBy
            {
                get { return this._ModifiedBy; }
                set
                {
                    if (value != this._ModifiedBy)
                    {
                        this._ModifiedBy = value;
                        NotifyPropertyChanged();
                    }
                }
            }


            /// <summary>
            /// Mật khẩu
            /// </summary>
            public string Password
            {
                get { return this._Password; }
                set
                {
                    if (value != this._Password)
                    {
                        this._Password = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            #endregion
        }
    }
}
