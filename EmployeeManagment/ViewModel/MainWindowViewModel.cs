using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using Prism.Commands;
using Prism.Mvvm;
using DataAccessLayer;
using EmployeeManagment.Utilities;

namespace EmployeeManagment.ViewModel
{
    class MainWindowViewModel : BindableBase, INotifyPropertyChanged
    {
        #region Properties

        private List<Employee> _employees;

        public List<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { SetProperty(ref _selectedEmployee, value); }
        }

        private bool _isLoadData;

        public bool IsLoadData
        {
            get { return _isLoadData; }
            set { SetProperty(ref _isLoadData, value); }
        }

        private string _responseMessage = "Welcome!!";

        public string ResponseMessage
        {
            get { return _responseMessage; }
            set { SetProperty(ref _responseMessage, value); }
        }

        #region [Create Employee Properties]

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _gender;

        public string Gender
        {
            get { return _gender; }
            set { SetProperty(ref _gender, value); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        #endregion
        
        private bool _isShowForm;

        public bool IsShowForm
        {
            get { return _isShowForm; }
            set { SetProperty(ref _isShowForm, value); }
        }

        private string _showPostMessage = "Fill the form to Add an employee!";

        public string ShowPostMessage
        {
            get { return _showPostMessage; }
            set { SetProperty(ref _showPostMessage, value); }
        }
        #endregion


        #region ICommands
        public DelegateCommand GetButtonClicked { get; set; }
        public DelegateCommand ShowRegistrationForm { get; set; }
        public DelegateCommand PostButtonClick { get; set; }
        public DelegateCommand<Employee> PutButtonClicked { get; set; }
        public DelegateCommand<Employee> DeleteButtonClicked { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initalize perperies & delegate commands
        /// </summary>
        public MainWindowViewModel()
        {
            GetButtonClicked = new DelegateCommand(GetEmployeeDetails);
            PutButtonClicked = new DelegateCommand<Employee>(UpdateEmployeeDetails);
            DeleteButtonClicked = new DelegateCommand<Employee>(DeleteEmployeeDetails);
            PostButtonClick = new DelegateCommand(CreateNewEmployee);
            ShowRegistrationForm = new DelegateCommand(RegisterEmployee);
        }
        #endregion

        #region CRUD       

        /// <summary>
        /// Make visible Regiter user form
        /// </summary>
        private void RegisterEmployee()
        {
            IsShowForm = true;
        }

        /// <summary>
        /// Fetches employee details
        /// </summary>
        private void GetEmployeeDetails()
        {
            var employeeDetails = WebAPI.GetCall(API_URIs.employees);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Employees = employeeDetails.Result.Content.ReadAsAsync<List<Employee>>().Result;
                IsLoadData = true;
            }
        }

        /// <summary>
        /// Adds new employee
        /// </summary>
        private void CreateNewEmployee()
        {
            Employee newEmployee = new Employee()
            {
                name = Name,
                email = Email,
                gender = Gender,
                status = Status
            };
            var employeeDetails = WebAPI.PostCall(API_URIs.employees, newEmployee);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                ShowPostMessage = newEmployee.name + "'s details has successfully been added!";
            }
            else
            {
                ShowPostMessage = "Failed to update" + newEmployee.name + "'s details.";
            }
        }


        /// <summary>
        /// Updates employee's record
        /// </summary>
        /// <param name="employee"></param>
        private void UpdateEmployeeDetails(Employee employee)
        {
            var employeeDetails = WebAPI.PutCall(API_URIs.employees + "?id=" + employee.Id, employee);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ResponseMessage = employee.name + "'s details has successfully been updated!";
            }
            else
            {
                ResponseMessage = "Failed to update" + employee.name + "'s details.";
            }
        }

        /// <summary>
        /// Deletes employee's record
        /// </summary>
        /// <param name="employee"></param>
        private void DeleteEmployeeDetails(Employee employee)
        {
            var employeeDetails = WebAPI.DeleteCall(API_URIs.employees + "?id=" + employee.Id);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ResponseMessage = employee.name + "'s details has successfully been deleted!";
            }
            else
            {
                ResponseMessage = "Failed to delete" + employee.name + "'s details.";
            }
        }
        #endregion
    }
}

