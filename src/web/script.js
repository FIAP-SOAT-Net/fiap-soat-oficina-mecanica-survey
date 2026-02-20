// Survey Application
(function() {
    'use strict';

    // Configuration
    const CONFIG = {
        API_ENDPOINT: (window.ENV_CONFIG && window.ENV_CONFIG.API_URL) || '/api/survey',
        API_TIMEOUT: 10000, // 10 seconds
        TOTAL_QUESTIONS: 5
    };

    // State
    const state = {
        customerName: '',
        customerEmail: '',
        answeredQuestions: new Set(),
        isSubmitting: false
    };

    // DOM Elements
    const elements = {
        form: document.getElementById('surveyForm'),
        customerName: document.getElementById('customerName'),
        submitBtn: document.getElementById('submitBtn'),
        progressCount: document.getElementById('progressCount'),
        progressFill: document.getElementById('progressFill'),
        successMessage: document.getElementById('successMessage'),
        errorMessage: document.getElementById('errorMessage'),
        missingParamsMessage: document.getElementById('missingParamsMessage'),
        errorText: document.getElementById('errorText'),
        retryBtn: document.getElementById('retryBtn'),
        comments: document.getElementById('comments'),
        charCount: document.getElementById('charCount')
    };

    // Initialize the application
    function init() {
        if (!parseURLParameters()) {
            showMissingParametersMessage();
            return;
        }

        displayCustomerName();
        setupEventListeners();
        updateProgress();
    }

    // Parse URL parameters
    function parseURLParameters() {
        const urlParams = new URLSearchParams(window.location.search);
        const nome = urlParams.get('nome');
        const email = urlParams.get('email');

        if (!nome || !email) {
            return false;
        }

        // Sanitize inputs to prevent XSS
        state.customerName = sanitizeHTML(nome);
        state.customerEmail = sanitizeHTML(email);

        // Validate email format
        if (!isValidEmail(state.customerEmail)) {
            console.error('Invalid email format');
            return false;
        }

        return true;
    }

    // Sanitize HTML to prevent XSS
    function sanitizeHTML(str) {
        const temp = document.createElement('div');
        temp.textContent = str;
        return temp.innerHTML;
    }

    // Validate email format
    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    // Display customer name
    function displayCustomerName() {
        elements.customerName.textContent = state.customerName;
    }

    // Show missing parameters message
    function showMissingParametersMessage() {
        elements.form.style.display = 'none';
        elements.missingParamsMessage.classList.add('show');
    }

    // Setup event listeners
    function setupEventListeners() {
        // Form submission
        elements.form.addEventListener('submit', handleSubmit);

        // Radio button changes
        for (let i = 1; i <= CONFIG.TOTAL_QUESTIONS; i++) {
            const radios = document.querySelectorAll(`input[name="q${i}"]`);
            radios.forEach(radio => {
                radio.addEventListener('change', () => handleQuestionAnswered(i));
            });
        }

        // Comments character count
        elements.comments.addEventListener('input', updateCharacterCount);

        // Retry button
        elements.retryBtn.addEventListener('click', handleRetry);
    }

    // Handle question answered
    function handleQuestionAnswered(questionNumber) {
        state.answeredQuestions.add(questionNumber);

        // Mark question block as answered
        const questionBlock = document.querySelector(`input[name="q${questionNumber}"]`).closest('.question-block');
        questionBlock.classList.add('answered');
        questionBlock.classList.remove('error');

        updateProgress();
    }

    // Update progress indicator
    function updateProgress() {
        const answeredCount = state.answeredQuestions.size;
        const percentage = (answeredCount / CONFIG.TOTAL_QUESTIONS) * 100;

        elements.progressCount.textContent = answeredCount;
        elements.progressFill.style.width = `${percentage}%`;
    }

    // Update character count for comments
    function updateCharacterCount() {
        const count = elements.comments.value.length;
        elements.charCount.textContent = count;

        if (count > 450) {
            elements.charCount.style.color = 'var(--warning-color)';
        } else {
            elements.charCount.style.color = 'var(--text-secondary)';
        }
    }

    // Handle form submission
    async function handleSubmit(event) {
        event.preventDefault();

        if (state.isSubmitting) {
            return;
        }

        // Validate all questions are answered
        if (!validateForm()) {
            return;
        }

        // Collect form data
        const surveyData = collectFormData();

        // Submit the survey
        await submitSurvey(surveyData);
    }

    // Validate form
    function validateForm() {
        let isValid = true;
        const errors = [];

        for (let i = 1; i <= CONFIG.TOTAL_QUESTIONS; i++) {
            const radios = document.querySelectorAll(`input[name="q${i}"]`);
            const questionBlock = radios[0].closest('.question-block');
            const isAnswered = Array.from(radios).some(radio => radio.checked);

            if (!isAnswered) {
                questionBlock.classList.add('error');
                errors.push(i);
                isValid = false;
            } else {
                questionBlock.classList.remove('error');
            }
        }

        if (!isValid) {
            // Scroll to first error
            const firstErrorBlock = document.querySelector('.question-block.error');
            if (firstErrorBlock) {
                firstErrorBlock.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
        }

        return isValid;
    }

    // Collect form data
    function collectFormData() {
        const data = {
            customerName: state.customerName,
            customerEmail: state.customerEmail,
            responses: {},
            comments: elements.comments.value.trim(),
            submittedAt: new Date().toISOString()
        };

        // Collect all ratings
        for (let i = 1; i <= CONFIG.TOTAL_QUESTIONS; i++) {
            const selected = document.querySelector(`input[name="q${i}"]:checked`);
            if (selected) {
                data.responses[`question${i}`] = parseInt(selected.value, 10);
            }
        }

        return data;
    }

    // Submit survey to API
    async function submitSurvey(data) {
        state.isSubmitting = true;
        setLoadingState(true);

        try {
            const controller = new AbortController();
            const timeoutId = setTimeout(() => controller.abort(), CONFIG.API_TIMEOUT);

            const response = await fetch(CONFIG.API_ENDPOINT, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
                signal: controller.signal
            });

            clearTimeout(timeoutId);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();
            console.log('Survey submitted successfully:', result);

            showSuccessMessage();
        } catch (error) {
            console.error('Error submitting survey:', error);
            handleSubmissionError(error);
        } finally {
            state.isSubmitting = false;
            setLoadingState(false);
        }
    }

    // Set loading state
    function setLoadingState(isLoading) {
        if (isLoading) {
            elements.submitBtn.classList.add('loading');
            elements.submitBtn.disabled = true;
        } else {
            elements.submitBtn.classList.remove('loading');
            elements.submitBtn.disabled = false;
        }
    }

    // Show success message
    function showSuccessMessage() {
        elements.form.style.display = 'none';
        elements.errorMessage.classList.remove('show');
        elements.successMessage.classList.add('show');

        // Scroll to top
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    // Handle submission error
    function handleSubmissionError(error) {
        let errorMessage = 'Ocorreu um erro ao enviar sua pesquisa. Por favor, tente novamente.';

        if (error.name === 'AbortError') {
            errorMessage = 'A requisição excedeu o tempo limite. Verifique sua conexão e tente novamente.';
        } else if (error.message.includes('Failed to fetch')) {
            errorMessage = 'Não foi possível conectar ao servidor. Verifique sua conexão com a internet.';
        } else if (error.message.includes('HTTP error')) {
            errorMessage = 'O servidor retornou um erro. Por favor, tente novamente mais tarde.';
        }

        elements.errorText.textContent = errorMessage;
        elements.errorMessage.classList.add('show');

        // Scroll to error message
        elements.errorMessage.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }

    // Handle retry
    function handleRetry() {
        elements.errorMessage.classList.remove('show');
        elements.form.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    // Start the application when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
