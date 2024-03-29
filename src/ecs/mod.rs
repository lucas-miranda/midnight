pub mod component;

pub mod entity;
pub mod system;

use std::collections::HashMap;
use strum::{EnumIter, IntoEnumIterator};
use crate::{time::DeltaTime, base::ApplicationState};

use self::{
    entity::Entities,
    system::{System, SystemInterface},
};

pub struct FrameState<'a> {
    pub delta: DeltaTime,
    pub app: &'a mut ApplicationState,
}


#[derive(PartialEq, Eq, Hash, EnumIter)]
pub enum SchedulerStep {
    Input,
    Update,
    Render,
}

pub struct SystemScheduler {
    systems: HashMap<SchedulerStep, Vec<system::SystemInterface>>,
}

impl SystemScheduler {
    pub fn new() -> Self {
        let mut systems = HashMap::default();

        for step in SchedulerStep::iter() {
            systems.insert(step, Vec::default());
        }

        Self {
            systems,
        }
    }

    pub fn register<S: 'static + System>(&mut self, step: SchedulerStep, sys: S) {
        let s = self.get_mut_step(&step);
        s.push(SystemInterface::wrap(sys));
    }

    pub fn run(&mut self, step: &SchedulerStep, entities: &Entities, state: &mut FrameState) {
        for sys in self.get_mut_step(step) {
            sys.run(entities.iter(), state)
        }
    }

    fn get_step(&self, step: &SchedulerStep) -> &Vec<system::SystemInterface> {
        self.systems.get(step).unwrap()
    }

    fn get_mut_step(&mut self, step: &SchedulerStep) -> &mut Vec<system::SystemInterface> {
        self.systems.get_mut(step).unwrap()
    }
}
