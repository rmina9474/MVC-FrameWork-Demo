// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Function to update cart count
function updateCartCount() {
    fetch('/ShoppingCart/GetCartCount')
        .then(response => response.json())
        .then(count => {
            const cartCountElement = document.querySelector('.cart-count');
            if (cartCountElement) {
                cartCountElement.textContent = count;
                
                // Add animation effect
                cartCountElement.classList.add('cart-count-update');
                setTimeout(() => {
                    cartCountElement.classList.remove('cart-count-update');
                }, 300);
            }
        })
        .catch(error => console.error('Error updating cart count:', error));
}

// Update cart count when page loads
document.addEventListener('DOMContentLoaded', updateCartCount);

// Update cart count every 30 seconds
setInterval(updateCartCount, 30000);

// Add to cart animation
function addToCart(productId) {
    fetch(`/ShoppingCart/AddToCart/${productId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            updateCartCount();
            // Show success message
            const toast = document.createElement('div');
            toast.className = 'toast-message success';
            toast.textContent = 'Added to cart successfully!';
            document.body.appendChild(toast);
            
            setTimeout(() => {
                toast.remove();
            }, 3000);
        }
    })
    .catch(error => console.error('Error adding to cart:', error));
}
