const RESTURL = 'https://direktedatarest2022.azurewebsites.net/api/DirekteData';

Vue.createApp({
    data() {
        return {
            DirekteData: [],
            dataStructure: {id: null, time: null, rotation: ""},
            idToGetBy: 0,
            dataToGet: 0,
            addData: {id: 0, time: 0, rotation: ""},
            addMessage: ""
        }
    },
    methods: {
        async getAll() {
            try {
                const response = await axios.get(RESTURL);
                this.DirekteData = await response.data;
            }
            catch (e) {
                alert(e.message);
            }
        },
        async getData(id) {
            try {
                const url = RESTURL + "/" + id;
                const response = await axios.get(url);
                this.dataStructure = await response.data;
            }
            catch (e) {
                this.dataStructure = null;
                // alert(e.message);
            }
        }
        // async add() {
        //     try {
        //         response = await axios.post(RESTURL, this.addData);
        //         this.addMessage = "Response: " + response.status + ", " + response.statusText;
        //         this.getAll();
        //     }
        //     catch (e) {
        //         alert(e.message);
        //     }
        // },
        // async addData(id) {
        //     try {
        //         const url = RESTURL + "/" + id;
        //         response = await axios.post(url);
        //         this.addMessage = "Response: " + response.status + ", " + response.statusText;
        //         this.getAll();
        //     }
        //     catch (e) {
        //         alert(e.message);
        //     }
        // }
    },
    created() {
        this.getAll();

    }
}).mount("#app")