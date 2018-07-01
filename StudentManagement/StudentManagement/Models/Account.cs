//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentManagement.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public partial class Account
    {
        public int AccountID { get; set; }
		[Required(ErrorMessage = "The User Name cannot be blank")]
		[Display(Name = "User Name")]
		public string AccountUserName { get; set; }
		[Required(ErrorMessage = "The Student name cannot be blank")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Student name between 3 and 50 characters in length")]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a Student name beginning with a capital letter and made up of letters and spaces only")]
		[Display(Name = "First Name")]
		public string Student_first_name { get; set; }
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Student name between 3 and 50 characters in length")]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a Student name beginning with a capital letter and made up of letters and spaces only")]
		[Display(Name = "Middle Name")]
		public string Student_middle_name { get; set; }
		[Required(ErrorMessage = "The Student name cannot be blank")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Student name between 3 and 50 characters in length")]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a Student name beginning with a capital letter and made up of letters and spaces only")]
		[Display(Name = "Last Name")]
		public string Student_last_name { get; set; }
		[Required(ErrorMessage = "The Employee name cannot be blank")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Employee name between 3 and 50 characters in length")]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a Employee name beginning with a capital letter and made up of letters and spaces only")]
		[Display(Name = "First Name")]
		public string Employee_first_name { get; set; }
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Employee name between 3 and 50 characters in length")]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a Employee name beginning with a capital letter and made up of letters and spaces only")]
		[Display(Name = "Middle Name")]
		public string Employee_middle_name { get; set; }
		[Required(ErrorMessage = "The Employee name cannot be blank")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Employee name between 3 and 50 characters in length")]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a Employee name beginning with a capital letter and made up of letters and spaces only")]
		[Display(Name = "Last Name")]
		public string Employee_last_name { get; set; }
		[Required(ErrorMessage = "The Date cannot be blank")]
		[Display(Name = "Date")]
		public Nullable<System.DateTime> Date { get; set; }
		[Required(ErrorMessage = "The Amount cannot be blank")]
		[Display(Name = "Amount")]
		public Nullable<decimal> Amount_Paid { get; set; }
        public string Remark { get; set; }
    }
}
