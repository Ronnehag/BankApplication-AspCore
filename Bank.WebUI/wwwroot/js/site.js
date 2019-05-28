

// Initializes all date pickers
(function () {
    $('.date-picker').datepicker({
        language: "sv",
        autoclose: "true",
        format: "yyyy-mm-dd",
    });
})();


(function (w, d) {
    const decimalInputs = d.querySelectorAll(".num-input");

    // Filter to allow only numbers and commas
    decimalInputs.forEach(input => {
        setInputFilter(input, function (value) {
            return /^\d*[,]?\d{0,2}$/.test(value);
        });
    });

    // Sets input filter for input box
    function setInputFilter(textbox, inputFilter) {
        ["input", "keydown", "keyup"].forEach(function (event) {
            textbox.addEventListener(event, function () {
                if (inputFilter(this.value)) {
                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                } else if (this.hasOwnProperty("oldValue")) {
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                }
            });
        });
    }

})(window, document);



