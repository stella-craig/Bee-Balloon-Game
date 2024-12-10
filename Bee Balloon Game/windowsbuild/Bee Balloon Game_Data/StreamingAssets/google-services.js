// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyDh_Fyh2i5MsGDbl9eqkLE3ZTnbOzuhzJo",
  authDomain: "gamedev-final-3b0a6.firebaseapp.com",
  projectId: "gamedev-final-3b0a6",
  storageBucket: "gamedev-final-3b0a6.firebasestorage.app",
  messagingSenderId: "704704840359",
  appId: "1:704704840359:web:884096807ce03b6eca1182",
  measurementId: "G-JE50P9PVJR"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);
