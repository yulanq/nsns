function loadSchedules(childId) {
    document.getElementById("selectedChildId").value = childId;

    if (!childId) {
        document.getElementById("scheduleTable").querySelector("tbody").innerHTML = "";
        return;
    }

    fetch(`/Coach/GetSchedules?childId=${childId}`)
        .then(response => response.json())
        .then(data => {
            const tbody = document.getElementById("scheduleTable").querySelector("tbody");
            tbody.innerHTML = "";
            data.forEach(schedule => {
                const row = `<tr>
                    <td>${schedule.courseTitle}</td>
                    <td>${schedule.scheduledAt}</td>
                    <td>${schedule.scheduledHours}</td>
                    <td>
                        <form method="post" action="/Coach/DeleteSchedule">
                            <input type="hidden" name="enrollmentId" value="${schedule.enrollmentID}" />
                            <button type="submit">Delete</button>
                        </form>
                    </td>
                </tr>`;
                tbody.innerHTML += row;
            });
        })
        .catch(error => console.error("Error loading schedules:", error));
}



//function fetchCoursesBySpecialty(specialtyId) {
//        if (!specialtyId) {
//            document.getElementById('courseId').innerHTML = '<option value="">-- Select Course --</option>';
//            return;
//        }

//        fetch(`/Child/GetCoursesBySpecialty?specialtyId=${specialtyId}`)
//            .then(response => response.json())
//            .then(data => {
//                const courseDropdown = document.getElementById('courseId');
//                courseDropdown.innerHTML = '<option value="">-- Select Course --</option>';
//                data.forEach(course => {
//                    const option = document.createElement('option');
//                    option.value = course.courseID;
//                    option.text = course.title;
//                    courseDropdown.add(option);
//                });
//            });
//    }


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