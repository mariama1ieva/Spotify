document.addEventListener('DOMContentLoaded', function () {
    const viewPremiumButton = document.getElementById('view-premium');
    const premiumCardsSection = document.getElementById('premium-cards');

    if (!viewPremiumButton || !premiumCardsSection) {
        console.error('Button or section not found.');
        return;
    }

    console.log('Button and section found.');

    viewPremiumButton.addEventListener('click', function () {
        console.log('Button clicked.');
        if (premiumCardsSection.style.display === 'none') {
            premiumCardsSection.style.display = 'block';
            console.log('Section shown.');
        } else {
            premiumCardsSection.style.display = 'none';
            console.log('Section hidden.');
        }
    });
});
