(function($){

    $.fn.generateFormField = function(fieldId, parameterDetails) {
        var paramHtml = '';
        switch(parameterDetails.formType) {
            case "password":
                paramHtml = $(`#${fieldId}`).generateTogglePasswordFormField(fieldId, parameterDetails);
                break;
            case "tags":
                paramHtml = $(`#${fieldId}`).generateTagsFormField({
                    containerId: `${fieldId}-container`,
                    valueContainerId: fieldId,
                    label: parameterDetails.displayName,
                    hiddenInputName: parameterDetails.name,
                    editorPlaceholder: 'Enter a name and press Enter',
                    isKeyValuePair: false
                });
                break;
            case "header-tags":
                paramHtml = $(`#${fieldId}`).generateTagsFormField({
                    containerId: `${fieldId}-container`,
                    valueContainerId: fieldId,
                    label: parameterDetails.displayName,
                    hiddenInputName: parameterDetails.name,
                    editorPlaceholder: 'Enter a Name:Value and press Enter',
                    isKeyValuePair: true
                });
                break;
            default:
                paramHtml = $(`#${fieldId}`).generateBasicFormField(fieldId, parameterDetails);
        }

        return paramHtml;
    };
    
    $.fn.generateBasicFormField = function(fieldId, parameterDetails) {
        const $containerDiv = $('<div>')
            .addClass('col-md-12 mb-3 control-group');
        
        const $inputGroup = $('<div>')
            .addClass('input-group');
        
        const $label = $('<label>')
            .attr('for', fieldId)
            .addClass('control-label')
            .text(parameterDetails.displayName);

        const $input = $('<input>')
            .attr({
                type: parameterDetails.formType,
                id: fieldId,
                name: parameterDetails.name,
                required: true
            })
            .addClass('form-control');

        $inputGroup.append($input);
        $containerDiv.append($label, $inputGroup);

        return $containerDiv;
    };

    $.fn.generateTogglePasswordFormField = function(fieldId, parameterDetails){
        const $containerDiv = $('<div>')
            .addClass('col-md-12 mb-3 control-group');
        
        const $inputGroup = $('<div>')
            .addClass('input-group');
        
        const $label = $('<label>')
            .attr('for', fieldId)
            .addClass('control-label')
            .text(parameterDetails.displayName);

        const $passwordInput = $('<input>')
            .attr({
                type: 'password',
                id: fieldId,
                name: parameterDetails.name,
                required: true
            })
            .addClass('form-control');

        // Create the toggle button
        const $toggleButton = $('<button>')
            .attr({
                type: 'button',
                id: `${fieldId}_togglePassword`
            })
            .addClass('btn btn-outline-secondary')
            .html('<i class="bi bi-eye-slash"></i>')
            .click(function () {
                const isPassword = $passwordInput.attr('type') === 'password';

                // Toggle the type of the input field
                $passwordInput.attr('type', isPassword ? 'text' : 'password');

                // Toggle the icon
                const $icon = $(this).find('i');
                $icon.toggleClass('bi-eye-slash bi-eye');
            });

        $inputGroup.append($passwordInput, $toggleButton);
        $containerDiv.append($label, $inputGroup);

        return $containerDiv;
    }

    $.fn.generateTagsFormField = function (options) {
        const settings = $.extend({
            // ID of the parent container where the tag system will be added
            containerId: '',
            // ID of the value container where the comma seperated tags will be stored.
            valueContainerId: '',
            // Label for the input field
            label: 'Tags',
            // Name attribute for the hidden input
            hiddenInputName: 'tags',
            // Placeholder for the editor input
            editorPlaceholder: 'Add tags and press Enter',
            // Determines if the tag is a key / value tag seperated with ':'
            isKeyValuePair: false
        }, options);

        const $parentContainer = $('<div>')
            .attr('id', settings.containerId)

        // Generate elements dynamically
        const editorId = `${settings.containerId}_editor`;
        const hiddenInputId = settings.valueContainerId;
        const tagContainerId = `${settings.containerId}_tags`;

        const $label = $('<label>')
            .attr('for', editorId)
            .addClass('control-label')
            .text(settings.label);

        const $editorInput = $('<input>')
            .attr({
                type: 'text',
                id: editorId,
                placeholder: settings.editorPlaceholder,
            })
            .addClass('form-control');

        const $hiddenInput = $('<input>')
            .attr({
                type: 'text',
                id: hiddenInputId,
                name: settings.hiddenInputName,
                required: true,
                hidden: true,
            })
            .addClass('form-control');

        const $tagContainer = $('<div>')
            .attr('id', tagContainerId)
            .addClass('col-md-12 mb-3')
            .css('display', 'flex')
            .css('flex-wrap', 'wrap')
            .css('gap', '0.2rem');

        const $inputContainer = $('<div>')
            .addClass('col-md-12 mb-3 control-group')
            .append($label)
            .append($editorInput)
            .append($hiddenInput);

        // Append the generated elements to the parent container
        $parentContainer.append($inputContainer).append($tagContainer);

        // Function to update the tag container display
        const updateTagsDisplay = () => {
            const tags = $hiddenInput.val().split(',').filter(t => t.trim());
            $tagContainer.empty(); // Clear the tag container

            tags.forEach(tag => {
                // Create a tag element
                const $tagElement = $('<span>')
                    .text(tag)
                    .addClass('badge-primary me-2');
                
                // Create a remove button
                const $removeButton = $('<span>')
                    .addClass('ms-2 text-danger cursor-pointer fw-bold')
                    .html('&times;')
                    .click(() => removeTag(tag)); // Attach click event to remove the tag

                // Append the remove button to the tag
                $tagElement.append($removeButton);

                // Append the tag element to the container
                $tagContainer.append($tagElement);
            });
        };

        // Function to add a new tag
        const addTag = (tag) => {
            const currentTags = $hiddenInput.val().split(',').filter(t => t.trim());
            if (!currentTags.includes(tag)) {
                currentTags.push(tag);
                $hiddenInput.val(currentTags.join(','));
                updateTagsDisplay();
            }
        };

        // Function to remove a tag
        const removeTag = (tag) => {
            let currentTags = $hiddenInput.val().split(',').filter(t => t.trim());
            currentTags = currentTags.filter(t => t !== tag);
            $hiddenInput.val(currentTags.join(','));
            updateTagsDisplay();
        };

        // Event listener for adding tags on Enter key press
        $editorInput.keypress(function (event) {
            if (event.which === 13) { // Enter key
                event.preventDefault(); // Prevent form submission
                
                //REMOVE ANY BLANK SPACES
                const tagValue = $(this).val().trim().replace(/\s/g, "");
                if (!(tagValue)) {
                    return;
                }
                
                if (settings.isKeyValuePair)
                {
                    //CHECK THAT THE KEY VALUE PAIR CONTAINS 1 SEPERATOR
                    if((tagValue.match(new RegExp(":", "g")) || []).length !== 1){
                        return;   
                    }
                    
                    //CHECK THAT BOTH KEY AND VALUE PAIR ARE NOT EMPTY STRING
                    var keys = tagValue.split(':');
                    if(keys[0] === '' || keys[1] === ''){
                        return;
                    }
                    
                    addTag(tagValue);
                    $(this).val(''); // Clear the editor input
                }else{
                    addTag(tagValue);
                    $(this).val(''); // Clear the editor input
                }
            }
        });

        // Event listener for when the main tag input changes
        $hiddenInput.change(function (event) {
            updateTagsDisplay();
        });

        // Initialize by rendering existing tags
        updateTagsDisplay();

        return $parentContainer;
    };
    
})( jQuery );