//import liraries
import React, { Component } from 'react';
import { View, Text, StyleSheet, TextInput, TouchableOpacity, StatusBar } from 'react-native';
import firebase from 'firebase';

// create a component
class LoginForm extends Component {
    state = { email: '', password: '', error: '' };
   
    componentWillMount() {
          // Initialize Firebase
        firebase.initializeApp({
            apiKey: 'AIzaSyCjZBhkksPMis6ypu4v-vUOgQSj43VF48A',
            authDomain: 'servicofood.firebaseapp.com',
            databaseURL: 'https://servicofood.firebaseio.com',
            projectId: 'servicofood',
            storageBucket: 'servicofood.appspot.com',
            messagingSenderId: '950742735356'
          });
    }
    //
    doLogin() {
        const { email, password } = this.state;
        //
        firebase.auth().signInWithEmailAndPassword(email, password)
        .then(this.onLoginSuccess.bind(this))
        .catch(() => {
            firebase.auth().createUserWithEmailAndPassword(email, password)
            .then(this.onLoginSuccess.bind(this))
            .catch(this.onLoginSuccess.bind(this));
        });
    }
    onLoginFailed() {
        this.setState(
            { 
                error: 'erro ao autenticar',
                loading: false
            }
        );
    }
    //
    onLoginSuccess() {
        this.setState({ 
            email: '', 
            password: '',
            error: '',
            loading: false
        });
    }
    //
    render() {
        return (
            <View style={styles.container}>
                <StatusBar barStyle="light-content" />

                <TextInput 
                        value={this.state.email}
                        onChangeText={(email) => this.setState({ email })}
                        placeholder="email"
                        placeholderTextColor="rgba(255,255,255,0.7)"
                        keyboardType="email-address"
                        autoCapitalize="none"
                        autoCorrect={false}
                        returnKeyType="next"
                        style={styles.input}
                        onSubmitEditing={() => { this.passwordInput.focus(); }}
                />
                <TextInput 
                    placeholder="senha"
                    value={this.state.password}
                    onChangeText={(password) => this.setState({ password })}
                    ref={(input) => this.passwordInput = input}
                    secureTextEntry
                    returnKeyType="go"
                    placeholderTextColor="rgba(255,255,255,0.7)" 
                    style={styles.input}
                />

                <TouchableOpacity onPress={this.doLogin()} style={styles.buttonContainer}>
                    <Text style={styles.buttonText}>
                        LOGIN
                    </Text>
                </TouchableOpacity>

                <Text style={styles.errorTextStyle}>{this.state.error}</Text>
            </View>
        );
    }
}

// define your styles
const styles = StyleSheet.create({
    container: {
        padding: 10,
        marginBottom: 70
    },
    input: {
        marginBottom: 10,
        height: 40,
        paddingHorizontal: 10,
        backgroundColor: 'rgba(255,255,255,0.2)', //esmaece
        color: '#FFFFFF'
    },
    buttonContainer: {
        backgroundColor: '#2980b9',
        paddingVertical: 15
    },
    buttonText: {
        color: '#FFFFFF',
        textAlign: 'center',
        fontWeight: '800'
       
    },
    errorTextStyle: {
        fontSize: 20,
        alignItems: 'center',
        color: 'red'
    }
});

//make this component available to the app
export default LoginForm;
