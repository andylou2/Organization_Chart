var ViewModel = function () {
    var self = this;
    self.departments = ko.observableArray();
    self.error = ko.observable();

    var departmentsUri = '/api/departments/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllDepartments() {
        ajaxHelper(departmentsUri, 'GET').done(function (data) {
            self.departments(data);
        });
    }

    self.detail = ko.observable();
    self.getDepartmentDetail = function (item) {
        ajaxHelper(departmentsUri + "getdepartment?id=" + item.ID, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    self.deleteDepartment = function (item) {
        ajaxHelper(departmentsUri + "deletedepartment?id=" + item.ID, 'DELETE').done(function (data) {
            self.detail(data);
        });
    }

    self.updateDepartmentDetail = function (item) {
        ajaxHelper(departmentsUri + "updatedepartment?id=" + item.ID, 'PUT').done(function (data) {
            self.detail(data);
        });
    }

    
    self.departments = ko.observableArray();
    self.newDepartment = {
        Name: ko.observable(),
        ParentDepartmentID: ko.observable()
    }

    self.addDepartment = function (formElement) {
        var dept = {
            Name: self.newDepartment.Name(),
            ParentDepartmentID: self.newDepartment.ParentDepartmentID()
        };

        ajaxHelper(departmentsUri + "createdepartment", 'POST', dept).done(function (item) {
            self.departments.push(item);
        });
    }


    //var employeesUri = '/api/employees/';

    //function getEmployees() {
    //    ajaxHelper(employeesUri, 'GET').done(function (data) {
    //        self.employees(data);
    //    });
    //}

    //getEmployees();


    // Fetch the initial data.
    getAllDepartments();
};

ko.applyBindings(new ViewModel());