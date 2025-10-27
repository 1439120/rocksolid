import { initializeApp } from "https://www.gstatic.com/firebasejs/10.12.2/firebase-app.js";
import { getAuth, signInWithEmailAndPassword, createUserWithEmailAndPassword, signOut, onAuthStateChanged } 
from "https://www.gstatic.com/firebasejs/10.12.2/firebase-auth.js";

const firebaseConfig = {
    apiKey: "AIzaSyAns3hvlW5l1QUzS5okDMi8A99-E6V2H_Q",
    authDomain: "blazor-project-41a39.firebaseapp.com",
    projectId: "blazor-project-41a39",
    storageBucket: "blazor-project-41a39.firebasestorage.app",
    messagingSenderId: "632676975269",
    appId: "1:632676975269:web:19d749a33db104204ac34c",
    measurementId: "G-26Y6J5LEHQ"
  };

  const app = initializeApp(firebaseConfig);
  const auth = getAuth(app);
  
  export async function register(email, password) {
      const userCredential = await createUserWithEmailAndPassword(auth, email, password);
      return userCredential.user.uid;
  }
  
  export async function login(email, password) {
      const userCredential = await signInWithEmailAndPassword(auth, email, password);
      return userCredential.user.uid;
  }
  
  export async function logout() {
      await signOut(auth);
  }
  
  export function onAuthChanged(callback) {
      onAuthStateChanged(auth, user => {
          callback(user ? user.email : null);
      });
  }