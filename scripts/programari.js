 // Available time slots for each day
    const timeSlots = {
      monday: [
        "09:00",
        "10:00",
        "11:00",
        "12:00",
        "14:00",
        "15:00",
        "16:00",
        "17:00",
      ],
      tuesday: [
        "09:00",
        "10:00",
        "11:00",
        "12:00",
        "14:00",
        "15:00",
        "16:00",
        "17:00",
      ],
      wednesday: [
        "09:00",
        "10:00",
        "11:00",
        "12:00",
        "14:00",
        "15:00",
        "16:00",
        "17:00",
      ],
      thursday: [
        "09:00",
        "10:00",
        "11:00",
        "12:00",
        "14:00",
        "15:00",
        "16:00",
        "17:00",
      ],
      friday: [
        "09:00",
        "10:00",
        "11:00",
        "12:00",
        "14:00",
        "15:00",
        "16:00",
        "17:00",
      ],
      saturday: ["10:00", "11:00", "12:00", "14:00", "15:00", "16:00"],
    };

    // Day names in Romanian
    const dayNames = {
      monday: "Luni",
      tuesday: "Marți",
      wednesday: "Miercuri",
      thursday: "Joi",
      friday: "Vineri",
      saturday: "Sâmbătă",
    };

    let selectedDay = null;
    let selectedTime = null;

    // Generate days for the next week
    function generateDays() {
      const container = document.getElementById("daysContainer");
      let dayKeys = Object.keys(timeSlots);
      const today = new Date();
      const currentDay = today.getDay();
      //rotire elemente din zile
      dayKeys = dayKeys.slice(currentDay).concat(dayKeys.slice(0, currentDay));
      console.log(dayKeys + " " + currentDay);
      dayKeys.forEach((dayKey, index) => {
        const date = new Date();
        date.setDate(date.getDate() + index);
        const dateString = date.toLocaleDateString("ro-RO", {
          day: "2-digit",
          month: "2-digit",
        });

        const button = document.createElement("button");
        button.type = "button";
        button.className = "day-btn";
        button.textContent = `${dayNames[dayKey]}\n${dateString}`;
        button.dataset.day = dayKey;

        button.addEventListener("click", function (e) {
          e.preventDefault();
          selectDay(this);
        });

        container.appendChild(button);
      });
    }

    // Select a day and populate time slots
    function selectDay(element) {
      // Remove previous selection
      document.querySelectorAll(".day-btn").forEach((btn) => {
        btn.classList.remove("active");
      });

      // Add active class to selected button
      element.classList.add("active");
      selectedDay = element.dataset.day;
      selectedTime = null;

      // Populate time slots
      populateTimeSlots(selectedDay);
    }

    // Populate time slots based on selected day
    function populateTimeSlots(day) {
      const container = document.getElementById("timesContainer");
      container.innerHTML = "";

      const times = timeSlots[day];
      times.forEach((time) => {
        const button = document.createElement("button");
        button.type = "button";
        button.className = "time-btn";
        button.textContent = time;
        button.dataset.time = time;

        button.addEventListener("click", function (e) {
          e.preventDefault();
          selectTime(this);
        });

        container.appendChild(button);
      });
    }

    // Select a time
    function selectTime(element) {
      // Remove previous selection
      document.querySelectorAll(".time-btn").forEach((btn) => {
        btn.classList.remove("active");
      });

      // Add active class to selected button
      element.classList.add("active");
      selectedTime = element.dataset.time;
    }

    // Handle form submission
    document
      .getElementById("bookingForm")
      .addEventListener("submit", function (e) {
        e.preventDefault();

        if (!selectedDay || !selectedTime) {
          alert("Te rog selectează o zi și o oră!");
          return;
        }

        const service = document.getElementById("service").value;
        const teamMemberSelect = document.getElementById("teamMember");
        const teamMemberId = teamMemberSelect.value;
        const teamMemberText =
          teamMemberSelect.options[teamMemberSelect.selectedIndex].text;

        let confirmationMessage = `Programare confirmată!\nServiciu: ${service}\nZiua: ${dayNames[selectedDay]}\nOra: ${selectedTime}`;

        if (teamMemberId) {
          confirmationMessage += `\nFrizer: ${teamMemberText}`;
        } else {
          confirmationMessage += `\nFrizer: Aleatoriu`;
        }

        alert(confirmationMessage);

        // Reset form
        this.reset();
        document.querySelectorAll(".day-btn, .time-btn").forEach((btn) => {
          btn.classList.remove("active");
        });
        selectedDay = null;
        selectedTime = null;
        document.getElementById("timesContainer").innerHTML =
          '<p class="text-muted">Alege mai întâi o zi</p>';
      });

    // Initialize on page load
    document.addEventListener("DOMContentLoaded", function () {
      generateDays();
    });