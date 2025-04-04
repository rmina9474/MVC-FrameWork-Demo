// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function() {
    // Check if we're in the admin area based on URL
    const isAdminPage = window.location.href.includes('/Admin/');
    
    // Check if the current user is an admin
    fetch('/api/User/IsAdmin')
        .then(response => response.json())
        .then(data => {
            // Hide cart icon for admin users or in admin area
            const cartIcon = document.querySelector('.cart-icon');
            if (cartIcon && (data.isAdmin || isAdminPage)) {
                cartIcon.style.display = 'none';
            }
            
            // Only update cart if not admin and not in admin area
            if (!data.isAdmin && !isAdminPage) {
                updateCartCount();
                // Update cart count every 30 seconds
                setInterval(updateCartCount, 30000);
            }
        })
        .catch(error => {
            console.error('Error checking admin status:', error);
            // Fallback to just URL-based detection if the API call fails
            const cartIcon = document.querySelector('.cart-icon');
            if (cartIcon && isAdminPage) {
                cartIcon.style.display = 'none';
            }
            
            if (!isAdminPage) {
                updateCartCount();
                setInterval(updateCartCount, 30000);
            }
        });
});

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
            // Update cart count
            updateCartCount();
            
            // Add animation to cart button
            const cartBtn = document.querySelector('.cart-btn');
            if (cartBtn) {
                cartBtn.classList.add('adding');
                setTimeout(() => {
                    cartBtn.classList.remove('adding');
                }, 800);
            }
            
            // Animate product to cart
            const productElement = document.querySelector(`[data-product-id="${productId}"]`);
            if (productElement) {
                const productRect = productElement.getBoundingClientRect();
                const cartRect = document.querySelector('.cart-icon').getBoundingClientRect();
                
                const ghostProduct = document.createElement('div');
                ghostProduct.className = 'ghost-product';
                ghostProduct.style.cssText = `
                    position: fixed;
                    z-index: 9999;
                    width: 50px;
                    height: 50px;
                    background-color: var(--primary);
                    border-radius: 50%;
                    opacity: 0.8;
                    pointer-events: none;
                    top: ${productRect.top + productRect.height/2}px;
                    left: ${productRect.left + productRect.width/2}px;
                    transform: translate(-50%, -50%);
                    transition: all 0.6s cubic-bezier(0.68, -0.55, 0.27, 1.55);
                `;
                
                document.body.appendChild(ghostProduct);
                
                // Trigger animation after a small delay
                setTimeout(() => {
                    ghostProduct.style.top = `${cartRect.top + cartRect.height/2}px`;
                    ghostProduct.style.left = `${cartRect.left + cartRect.width/2}px`;
                    ghostProduct.style.opacity = '0';
                    ghostProduct.style.transform = 'translate(-50%, -50%) scale(0.3)';
                }, 10);
                
                // Remove ghost element after animation completes
                setTimeout(() => {
                    ghostProduct.remove();
                }, 650);
            }
            
            // Show success message
            const toast = document.createElement('div');
            toast.className = 'toast-message success';
            toast.innerHTML = `
                <i class="bi bi-check-circle me-2"></i>
                Item added to cart successfully!
            `;
            document.body.appendChild(toast);
            
            setTimeout(() => {
                toast.classList.add('fade-out');
                setTimeout(() => {
                    toast.remove();
                }, 300);
            }, 2700);
        }
    })
    .catch(error => console.error('Error adding to cart:', error));
}
