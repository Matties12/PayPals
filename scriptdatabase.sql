-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8mb3 ;
-- -----------------------------------------------------
-- Schema zoostart
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema zoostart
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `zoostart` ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`diertussen`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`diertussen` (
  `idDierTussen` INT NOT NULL AUTO_INCREMENT,
  `fkWoonst` INT NOT NULL,
  `fkdier` INT NOT NULL,
  PRIMARY KEY (`idDierTussen`),
  INDEX `fkdier` (`fkdier` ASC) VISIBLE,
  INDEX `fkWoonst` (`fkWoonst` ASC) VISIBLE,
  CONSTRAINT `diertussen_ibfk_1`
    FOREIGN KEY (`fkdier`)
    REFERENCES `zoostart`.`dieren` (`idDieren`),
  CONSTRAINT `diertussen_ibfk_2`
    FOREIGN KEY (`fkWoonst`)
    REFERENCES `zoostart`.`dierwoonst` (`idDierWoonst`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `mydb`.`dierwoonst`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`dierwoonst` (
  `idDierWoonst` INT NOT NULL AUTO_INCREMENT,
  `Locatie` VARCHAR(255) NOT NULL,
  `Binnen` TINYINT(1) NOT NULL,
  PRIMARY KEY (`idDierWoonst`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `mydb`.`gebruiker`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`gebruiker` (
  `idGebruiker` INT NOT NULL AUTO_INCREMENT,
  `VoornaamGebruiker` VARCHAR(45) NOT NULL,
  `AchternaamGebruiker` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`idGebruiker`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `mydb`.`leerkracht`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`leerkracht` (
  `idLeerkracht` INT NOT NULL AUTO_INCREMENT,
  `NaamLeerkracht` VARCHAR(45) NOT NULL,
  `FamilienaamLeerkracht` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idLeerkracht`))
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `mydb`.`leerling`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`leerling` (
  `idLeerling` INT NOT NULL AUTO_INCREMENT,
  `Naamleerling` VARCHAR(45) NOT NULL,
  `FamilienaamLeerling` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idLeerling`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `mydb`.`leerkracht_has_leerling`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`leerkracht_has_leerling` (
  `FKLeerkracht` INT NOT NULL,
  `FKLeerling` INT NOT NULL,
  `DagvanDeWeek` VARCHAR(45) NOT NULL,
  `Lokaal` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`FKLeerkracht`, `FKLeerling`),
  INDEX `fk_Leerkracht_has_Leerling_Leerling1_idx` (`FKLeerling` ASC) VISIBLE,
  INDEX `fk_Leerkracht_has_Leerling_Leerkracht_idx` (`FKLeerkracht` ASC) VISIBLE,
  CONSTRAINT `fk_Leerkracht_has_Leerling_Leerkracht`
    FOREIGN KEY (`FKLeerkracht`)
    REFERENCES `mydb`.`leerkracht` (`idLeerkracht`),
  CONSTRAINT `fk_Leerkracht_has_Leerling_Leerling1`
    FOREIGN KEY (`FKLeerling`)
    REFERENCES `mydb`.`leerling` (`idLeerling`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `mydb`.`lijst`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`lijst` (
  `idLijst` INT NOT NULL AUTO_INCREMENT,
  `NaamLijst` VARCHAR(45) NOT NULL,
  `DatumLijst` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`idLijst`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

USE `zoostart` ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
