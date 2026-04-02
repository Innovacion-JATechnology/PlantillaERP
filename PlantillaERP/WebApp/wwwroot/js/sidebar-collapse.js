// Sidebar Collapse Handler
(function() {
    'use strict';

    console.log('[Sidebar] Script externo cargado');

    function init() {
        console.log('[Sidebar] Inicializando...');

        const btn = document.getElementById('sidebarCollapseToggle');
        const sidebar = document.querySelector('.sb-sidenav');
        const content = document.getElementById('layoutSidenav_content');

        console.log('[Sidebar] Elementos:', { btn: !!btn, sidebar: !!sidebar, content: !!content });

        if (!btn || !sidebar || !content) {
            console.error('[Sidebar] ERROR: Elementos no encontrados');
            return;
        }

        btn.addEventListener('click', function(e) {
            e.preventDefault();
            console.log('[Sidebar] CLICK detectado');

            const isCollapsed = document.body.classList.contains('sb-sidenav-collapsed');
            console.log('[Sidebar] Collapsed?', isCollapsed);

            if (isCollapsed) {
                // Expand to 225px
                document.body.classList.remove('sb-sidenav-collapsed');
                sidebar.style.setProperty('width', '225px', 'important');
                content.style.setProperty('margin-left', '225px', 'important');
                console.log('[Sidebar] EXPANDIDO a 225px');
            } else {
                // Collapse to 70px
                document.body.classList.add('sb-sidenav-collapsed');
                sidebar.style.setProperty('width', '70px', 'important');
                content.style.setProperty('margin-left', '70px', 'important');
                console.log('[Sidebar] CONTRAÍDO a 70px');
            }

            updateIcon();
        });

        console.log('[Sidebar] Evento click adjuntado');
    }

    function updateIcon() {
        const icon = document.querySelector('#sidebarCollapseToggle i');
        if (icon) {
            icon.className = document.body.classList.contains('sb-sidenav-collapsed') 
                ? 'fas fa-bars' 
                : 'fas fa-bars';
        }
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
