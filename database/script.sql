CREATE DATABASE IF NOT EXISTS job_candidates;
USE job_candidates;

CREATE TABLE `Candidate` (
  `id` char(36) NOT NULL,
  `full_name` varchar(150) NOT NULL,
  `date_of_birth` date NOT NULL,
  `email` varchar(255) NOT NULL,
  `contact_num` varchar(30) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_candidate_email` (`email`),
  KEY `idx_candidate_full_name` (`full_name`)
);

CREATE TABLE `Skill` (
  `id` char(36) NOT NULL,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_skill_name` (`name`)
);

CREATE TABLE `Candidate_skills` (
  `candidate_id` char(36) NOT NULL,
  `skill_id` char(36) NOT NULL,
  PRIMARY KEY (`candidate_id`, `skill_id`),
  KEY `idx_candidate_skills_skill_id` (`skill_id`),
  CONSTRAINT `fk_candidate_skills_candidate`
    FOREIGN KEY (`candidate_id`) REFERENCES `Candidate` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_candidate_skills_skill`
    FOREIGN KEY (`skill_id`) REFERENCES `Skill` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

USE job_candidates;

INSERT INTO `Candidate` (`id`, `full_name`, `date_of_birth`, `email`, `contact_num`) VALUES
('11111111-1111-1111-1111-111111111111', 'Ana Jovanovic',  '1998-03-12', 'ana.jovanovic@mail.com',   '+38164111222'),
('22222222-2222-2222-2222-222222222222', 'Marko Petrovic', '1995-11-02', 'marko.petrovic@mail.com',  '+381631234567'),
('33333333-3333-3333-3333-333333333333', 'Ivana Nikolic',  '2000-07-25', 'ivana.nikolic@mail.com',   '+38160123456');

INSERT INTO `Skill` (`id`, `name`) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Java programming'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'C# programming'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Database design'),
('dddddddd-dddd-dddd-dddd-dddddddddddd', 'English'),
('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'German language'),
('ffffffff-ffff-ffff-ffff-ffffffffffff', 'Russian language');

INSERT INTO `Candidate_skills` (`candidate_id`, `skill_id`) VALUES
('11111111-1111-1111-1111-111111111111', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'), 
('11111111-1111-1111-1111-111111111111', 'cccccccc-cccc-cccc-cccc-cccccccccccc'),
('11111111-1111-1111-1111-111111111111', 'dddddddd-dddd-dddd-dddd-dddddddddddd'),
('22222222-2222-2222-2222-222222222222', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'), 
('22222222-2222-2222-2222-222222222222', 'cccccccc-cccc-cccc-cccc-cccccccccccc'), 
('22222222-2222-2222-2222-222222222222', 'dddddddd-dddd-dddd-dddd-dddddddddddd'), 
('22222222-2222-2222-2222-222222222222', 'ffffffff-ffff-ffff-ffff-ffffffffffff'), 
('33333333-3333-3333-3333-333333333333', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'), 
('33333333-3333-3333-3333-333333333333', 'dddddddd-dddd-dddd-dddd-dddddddddddd'), 
('33333333-3333-3333-3333-333333333333', 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'); 