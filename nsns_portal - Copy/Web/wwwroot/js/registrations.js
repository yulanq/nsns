

function fetchCoursesBySpecialty(specialtyId) {
        if (!specialtyId) {
            document.getElementById('courseId').innerHTML = '<option value="">-- Select Course --</option>';
            return;
        }

        fetch(`/Child/GetCoursesBySpecialty?specialtyId=${specialtyId}`)
            .then(response => response.json())
            .then(data => {
                const courseDropdown = document.getElementById('courseId');
                courseDropdown.innerHTML = '<option value="">-- Select Course --</option>';
                data.forEach(course => {
                    const option = document.createElement('option');
                    option.value = course.courseID;
                    option.text = course.title;
                    courseDropdown.add(option);
                });
            });
    }


/*
    function fetchCoaches(specialtyId) {
    if (!specialtyId) {
        document.getElementById('CoachID').innerHTML = '<option value="">-- Select Coach --</option>';
        return;
    }

    fetch(`/Course/GetCoachesBySpecialty?specialtyId=${specialtyId}`)
        .then(response => response.json())
        .then(data => {
            const coachDropdown = document.getElementById('UserID');
            coachDropdown.innerHTML = '<option value="">-- Select Coach --</option>';
            data.forEach(coach => {
                const option = document.createElement('option');
                option.value = coach.userID;
                option.text = coach.name;
                coachDropdown.add(option);
            });
        });
}
*/