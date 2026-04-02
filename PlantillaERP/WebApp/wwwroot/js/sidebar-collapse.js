// Sidebar Collapse Handler - Robust Version with Event Delegation
(function() {
    'use strict';

    console.log('[Sidebar] Script loaded');

    // Use event delegation on document level for maximum compatibility
    document.addEventListener('click', function(e) {
        // Check if the clicked element is the collapse button or a child of it
        const btn = e.target.closest('#sidebarCollapseToggle');

        if (!btn) return; // Not our button, ignore

        e.preventDefault();
        console.log('[Sidebar] Button clicked');

        // Find sidebar and content elements
        const sidebar = document.querySelector('.sb-sidenav');
        const content = document.getElementById('layoutSidenav_content');

        if (!sidebar || !content) {
            console.error('[Sidebar] ERROR: sidebar or content element not found', { 
                sidebar: !!sidebar, 
                content: !!content 
            });
            return;
        }

        // Toggle the collapsed state
        const isCollapsed = document.body.classList.contains('sb-sidenav-collapsed');
        console.log('[Sidebar] Current state - Collapsed?', isCollapsed);

        if (isCollapsed) {
            // Expand sidebar
            document.body.classList.remove('sb-sidenav-collapsed');
            sidebar.style.width = '225px';
            sidebar.style.setProperty('width', '225px', 'important');
            content.style.setProperty('padding-left', '225px', 'important');
            console.log('[Sidebar] Expanded to 225px');
        } else {
            // Collapse sidebar
            document.body.classList.add('sb-sidenav-collapsed');
            sidebar.style.width = '70px';
            sidebar.style.setProperty('width', '70px', 'important');
            content.style.setProperty('padding-left', '70px', 'important');
            console.log('[Sidebar] Collapsed to 70px');
        }

        updateIcon();
    }, true); // Use capture phase to ensure we catch the click

    function updateIcon() {
        const icon = document.querySelector('#sidebarCollapseToggle i');
        if (icon) {
            const isCollapsed = document.body.classList.contains('sb-sidenav-collapsed');
            icon.className = isCollapsed ? 'fas fa-bars' : 'fas fa-bars';
        }
    }

    // Ensure proper state when page loads
    function initializeState() {
        console.log('[Sidebar] Checking initial state...');

        const sidebar = document.querySelector('.sb-sidenav');
        const content = document.getElementById('layoutSidenav_content');

        if (sidebar && content) {
            const isCollapsed = document.body.classList.contains('sb-sidenav-collapsed');
            console.log('[Sidebar] Initial state - Collapsed?', isCollapsed);

            if (isCollapsed) {
                sidebar.style.setProperty('width', '70px', 'important');
                content.style.setProperty('padding-left', '70px', 'important');
            } else {
                sidebar.style.setProperty('width', '225px', 'important');
                content.style.setProperty('padding-left', '225px', 'important');
            }
        }
    }

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeState);
    } else {
        // DOM already ready
        initializeState();
    }

    // Also check on page visibility change (when returning to tab)
    document.addEventListener('visibilitychange', function() {
        if (document.visibilityState === 'visible') {
            console.log('[Sidebar] Page became visible, re-checking state');
            initializeState();
        }
    });

    console.log('[Sidebar] Event delegation initialized');
})();
