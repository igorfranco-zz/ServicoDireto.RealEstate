//import liraries
import React, { Component } from 'react';
import { View, Text, Image, StyleSheet } from 'react-native';

// create a component
class Splash extends Component {
    render() {
        return (
            <View style={styles.container}>
                <Image source={require('../_images/logo@2.png')} /> 
                <Text>Splash</Text>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
      backgroundColor: '#fff',
      alignItems: 'center',
      justifyContent: 'center',
    },
  });
  
//make this component available to the app
export default Splash;
