import { PLATFORM } from "aurelia-framework";
import { Router, RouterConfiguration } from "aurelia-router";


import { library, dom } from '@fortawesome/fontawesome-svg-core'
import { faHome, faUsers } from '@fortawesome/free-solid-svg-icons'

library.add(faHome, faUsers);

// Kicks off the process of finding <i> tags and replacing with <svg>
dom.watch();

export class App {
	configureRouter(config: RouterConfiguration, router: Router) {
        config.title = "Health Catalyst Sample App";
		config.map([{
			route: ["", "home"],
			name: "home",
			settings: { icon: faHome.iconName, prefix: faHome.prefix },
			moduleId: PLATFORM.moduleName("../../components/pages/home/home"),
			nav: true,
			title: "Home"
        }, {
            route: 'person',
                name: 'person',
                settings: { icon: faUsers.iconName, prefix: faUsers.prefix },
                moduleId: PLATFORM.moduleName('../../components/pages/person/person'),
            nav: true,
            title: 'People Search'
        }]);
	}
}
