setTimeout(function () {
    let successMessage = document.querySelector(".successMessage");
    let sucessMessageUser = document.querySelector(".successMessageUser");
    if (successMessage || sucessMessageUser) {
        successMessage.style.display = 'none';
        sucessMessageUser.style.display = 'none';
    }
}, 5000);


function enlargeEmployeeImage() {
    let employeeImage = document.querySelector(".employeeImage");
    employeeImage.style.transform = 'scale(1.5)';
    employeeImage.style.width = '200px';
    employeeImage.style.height = '200px';
    employeeImage.style.transition = "transform 0.5s ease";
}